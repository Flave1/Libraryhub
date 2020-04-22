using Libraryhub.AppEnum;
using Libraryhub.Contracts.Commands; 
using Libraryhub.Contracts.RequestObjs;
using Libraryhub.Contracts.Response;
using Libraryhub.DomainObjs;
using Libraryhub.ErrorHandler;
using Libraryhub.Service.Services;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System; 
using System.Threading;
using System.Threading.Tasks;

namespace Libraryhub.Handlers.Books
{
    public class CheckInHandler : IRequestHandler<CheckInCommand, CheckOutActivityResponseObj>
    {
        private readonly IBookService _bookService; 
        private readonly ILogger _logger;

        public CheckInHandler(IBookService bookService, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(typeof(CheckInHandler));
            _bookService = bookService;
        }

        public async Task<CheckOutActivityResponseObj> Handle(CheckInCommand request, CancellationToken cancellationToken)
        {
            try
            {
                #region Check if the book to be checked in is available and has a book activity with status CHECKEDOUT

                var thisBook = await _bookService.GetBookByIdAsync(request.BookId);
                if (thisBook == null)
                {
                    return new CheckOutActivityResponseObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = $"Unable to find checkout activity for this book"
                            }
                        }
                    };
                }

                if (thisBook.Quantity == thisBook.InitialQuantity)
                {
                    return new CheckOutActivityResponseObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = $"Book is not currently checked out"
                            }
                        }
                    };
                }

                var currentCheckedOutBook = await _bookService.GetCheckOutActivityById(request.CheckOutActivityId);
                if (currentCheckedOutBook == null)
                {
                    return new CheckOutActivityResponseObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = $"Unable to find checkout activity for this book"
                            }
                        }
                    };
                }

                if (currentCheckedOutBook.Status == (int)BookActivityStatus.Check_In)
                {
                    return new CheckOutActivityResponseObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = $"You have already checked in this book"
                            }
                        }
                    };
                }
                #endregion

                #region Check if customer exceeded the expected date to return this book and also penalize customer

                var checkInReq = BuildCheckInDomainObject(currentCheckedOutBook, request);

                if (request.ReturnDate > currentCheckedOutBook.ExpectedReturnDate)
                {
                    var lateDays = Convert.ToInt64((request.ReturnDate - currentCheckedOutBook.ExpectedReturnDate).TotalDays);
                    var penaltyFee = 200 * lateDays;


                    if (request.PenaltyFee == null || request.PenaltyFee == 0)
                        return new CheckOutActivityResponseObj
                        {
                            Status = new APIResponseStatus
                            {
                                IsSuccessful = false,
                                Message = new APIResponseMessage
                                {
                                    FriendlyMessage = $"Customer expected to pay a penalty fee of {penaltyFee} for {lateDays} day(s) late return"
                                }
                            }
                        };

                    if (penaltyFee != request.PenaltyFee && request.PenaltyFee != 0)
                    {
                        return new CheckOutActivityResponseObj
                        {
                            Status = new APIResponseStatus
                            {
                                IsSuccessful = false,
                                Message = new APIResponseMessage
                                {
                                    FriendlyMessage = $"Customer expected to pay a penalty fee of {penaltyFee} for {lateDays} day(s) late return not {request.PenaltyFee}"
                                }
                            }
                        };
                    }

                    var penaltyObject = BuildPenaltyDomainObject(currentCheckedOutBook, request, penaltyFee, lateDays);

                    using (var _transaction = _bookService.Context().Database.BeginTransaction())
                    {
                        try
                        {
                            await _bookService.PenalizeAsync(penaltyObject);

                            thisBook.IsAvailable = true;
                            thisBook.Quantity = thisBook.Quantity + 1;
                            await _bookService.UpdateBookAsync(thisBook);
                            await _bookService.SaveChangesAsync();

                            await _bookService.CheckInBookAsync(checkInReq);
                            await _bookService.SaveChangesAsync();
                            await _transaction.CommitAsync();
                           
                        }
                        catch (SqlException ex)
                        {
                            #region Log Error with errorId and return error resonse 

                            await _transaction.RollbackAsync();
                            var errorId = ErrorID.Generate(4);
                            _logger.LogInformation($"CheckOutBookHandler{errorId}", $"Error Message{ ex.InnerException?.Message ?? ex?.Message}");
                            return new CheckOutActivityResponseObj
                            {
                                Status = new APIResponseStatus
                                {
                                    IsSuccessful = false,
                                    Message = new APIResponseMessage
                                    {
                                        FriendlyMessage = "Something went wrong",
                                        MessageId = $"CheckOutBookHandler{errorId}",
                                        TechnicalMessage = ex.InnerException?.Message ?? ex?.Message
                                    }
                                }
                            };
                            #endregion
                        }
                        finally
                        {
                            await _transaction.DisposeAsync();
                        }

                    }

                    return new CheckOutActivityResponseObj
                    {
                        CheckOutActivityId = checkInReq.CheckOutActivityId,
                        Status = new APIResponseStatus { IsSuccessful = true }
                    };
                }
                #endregion

                #region Save changes if customer did not exceed expected date of return

                await _bookService.CheckInBookAsync(checkInReq);
                await _bookService.SaveChangesAsync();
                return new CheckOutActivityResponseObj
                {
                    CheckOutActivityId = checkInReq.CheckOutActivityId,
                    Status = new APIResponseStatus { IsSuccessful = true }
                };

                #endregion
            }
            catch (Exception ex)
            {
                #region Log Error with errorId and return error resonse 
                var errorId = ErrorID.Generate(4);
                _logger.LogInformation($"CheckInHandler{errorId}", $"Error Message{ ex.InnerException?.Message ?? ex?.Message}");
                return new CheckOutActivityResponseObj
                {
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = false,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = " Something went wrong",
                            MessageId = $"CheckInHandler{errorId}",
                            TechnicalMessage = ex.InnerException?.Message ?? ex?.Message
                        }
                    }
                };
                #endregion
            }
        }

        #region Build domain objects

        private BooksActivity BuildCheckInDomainObject(BooksActivity currentCheckedOutBook, CheckInCommand request)
        {
            var checkInReq = new BooksActivity
            {
                CustomerId = currentCheckedOutBook.CustomerId,
                BookId = request.BookId,
                CheckOutDate = currentCheckedOutBook.CheckOutDate,
                Email = currentCheckedOutBook.Email,
                ExpectedReturnDate = currentCheckedOutBook.ExpectedReturnDate,
                FullName = currentCheckedOutBook.FullName,
                NationalIdentificationNumber = currentCheckedOutBook.NationalIdentificationNumber,
                PhoneNumber = currentCheckedOutBook.PhoneNumber,
                ReturnDate = DateTime.Today,
                Status = (int)BookActivityStatus.Check_In,
                CheckOutActivityId = request.CheckOutActivityId,
                AdminUserId = request.AdminUserId

            };
            return checkInReq;
        }

        private BookPenalty BuildPenaltyDomainObject(BooksActivity currentCheckedOutBook, CheckInCommand request, decimal penaltyFee, long lateDays)
        {
            var penaltyReq = new BookPenalty
            {
                ExpectedReturnDate = currentCheckedOutBook.ExpectedReturnDate,
                NumberOfDaysLate = Convert.ToInt64(lateDays),
                CheckOutActivityId = request.CheckOutActivityId,
                BookId = request.BookId,
                PenaltyFee = penaltyFee,
                CustomerId = currentCheckedOutBook.CustomerId,
                AdminUserId = request.AdminUserId
            };
            return penaltyReq;
        }

        #endregion
    }
}
