using Libraryhub.AppEnum;
using Libraryhub.Contracts.Commands; 
using Libraryhub.Contracts.RequestObjs;
using Libraryhub.Contracts.Response;
using Libraryhub.DomainObjs;
using Libraryhub.ErrorHandler;
using Libraryhub.Service.Services;
using MediatR; 
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
                if(currentCheckedOutBook == null)
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

                var checkInReq = BuildCheckInRequestObject(currentCheckedOutBook, request);

                if (request.ReturnDate.AddDays(1) > currentCheckedOutBook.ExpectedReturnDate)
                { 
                    var lateDays = Convert.ToInt64((request.ReturnDate.AddDays(1) - currentCheckedOutBook.ExpectedReturnDate).TotalDays);
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
                                    FriendlyMessage = $"Customer expected to pay a penalty fee of {penaltyFee} for {Convert.ToInt64(lateDays)} day(s) late return not {request.PenaltyFee}"
                                }
                            }
                        };
                    }

                    var penaltyReq = BuildPenaltyRequestObject(currentCheckedOutBook, request, penaltyFee, lateDays);

                    await _bookService.PenalizeAsync(penaltyReq);

                    thisBook.IsAvailable = true;
                    thisBook.Quantity = thisBook.Quantity + 1;
                    await _bookService.UpdateBookAsync(thisBook);

                    await _bookService.CheckInBookAsync(checkInReq);
                    return new CheckOutActivityResponseObj
                    {
                        CheckOutActivityId = checkInReq.CheckOutActivityId,
                        Status = new APIResponseStatus { IsSuccessful = true }
                    };


                }
                await _bookService.CheckInBookAsync(checkInReq);
                return new CheckOutActivityResponseObj
                {
                    CheckOutActivityId = checkInReq.CheckOutActivityId,
                    Status = new APIResponseStatus { IsSuccessful = true }
                };
            }
            catch (Exception ex)
            {
                var errorId = ErrorID.Generate(4);
                _logger.LogInformation( $"CheckInHandler{errorId}", $"Error Message{ ex.InnerException.Message}");
                return new CheckOutActivityResponseObj
                { 
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = false,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = " Something went wrong",
                            MessageId = $"CheckInHandler{errorId}",
                            TechnicalMessage = ex.InnerException.Message
                        }
                    }
                };
            }
        }

        private BooksActivity BuildCheckInRequestObject(BooksActivity currentCheckedOutBook, CheckInCommand request)
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

        private BookPenalty BuildPenaltyRequestObject(BooksActivity currentCheckedOutBook, CheckInCommand request, decimal penaltyFee, long lateDays)
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
    }
}
