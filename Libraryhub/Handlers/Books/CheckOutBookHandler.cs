using Libraryhub.AppEnum;
using Libraryhub.Contracts.Commands;
using Libraryhub.Contracts.RequestObjs;
using Libraryhub.Contracts.Response;
using Libraryhub.Domain;
using Libraryhub.DomainObjs;
using Libraryhub.ErrorHandler;
using Libraryhub.MailHandler;
using Libraryhub.MailHandler.Service;
using Libraryhub.Service.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Libraryhub.Handlers.Books
{
    public class CheckOutBookHandler : IRequestHandler<CheckOutCommand, CheckOutActivityResponseObj>
    {
        private readonly IBookService _bookService; 
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger; 
        private readonly IEmailService _emailService;

        public CheckOutBookHandler(IBookService bookService, UserManager<ApplicationUser> userManager, 
            ILoggerFactory loggerFactory, IEmailService emailService)
        {
            _bookService = bookService; 
            _userManager = userManager;
            _logger = loggerFactory.CreateLogger(typeof(CheckOutBookHandler));
            _emailService = emailService;
        }

        public async Task<CheckOutActivityResponseObj> Handle(CheckOutCommand request, CancellationToken cancellationToken)
        {
            try
            {
                #region Check if requested book is available in the library
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
                                FriendlyMessage = "This book is not found in the Library"
                            }
                        }
                    };
                }
                

                if (!thisBook.IsAvailable)
                {
                    return new CheckOutActivityResponseObj
                    {
                        Status = new APIResponseStatus  { 
                            IsSuccessful = false,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = "This book is not available to be checked out"
                            } 
                        }
                    };
                }
                #endregion 

                #region Check out requested book and update book activity

                var customer = await _userManager.FindByIdAsync(request.CustomerId);

                var checkOut = BuildDomainObject(request, customer);
                using (var _transaction = _bookService.Context().Database.BeginTransaction())
                {
                    try
                    {
                        thisBook.Quantity = (thisBook.Quantity - 1);
                        if (thisBook.Quantity == 0) { thisBook.IsAvailable = false; }
                        await _bookService.UpdateBookAsync(thisBook);
                        await _bookService.SaveChangesAsync();

                        await _bookService.CheckOutBookAsync(checkOut);
                        await _bookService.SaveChangesAsync();

                        await _transaction.CommitAsync();
                    }
                    catch (SqlException ex)
                    {
                        await _transaction.RollbackAsync();
                        var errorId = ErrorID.Generate(4);
                        _logger.LogInformation($"CheckInBookHandler{errorId}", $"Error Message{ ex.InnerException?.Message ?? ex?.Message}");
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

                    }
                    finally { await _transaction.DisposeAsync(); }
                }

                #endregion

                #region Send mail to the customer leting him know the Expected date to return book collected
                var numberOfdays = Convert.ToInt32((checkOut.ExpectedReturnDate - checkOut.CheckOutDate).TotalDays);

                if (!await SendMail(request, thisBook.Title, checkOut.ExpectedReturnDate, numberOfdays)) { };

                return new CheckOutActivityResponseObj
                {
                    CheckOutActivityId = checkOut.CheckOutActivityId,
                    Status = new APIResponseStatus { IsSuccessful = true }
                };
                #endregion

            }
            catch (Exception ex)
            {
                #region Log eroor with ErroId and return error response
                var errorId = ErrorID.Generate(4);
                _logger.LogInformation($"CheckOutBookHandler{errorId}", $"Error Message{ ex.InnerException.Message}");
                return new CheckOutActivityResponseObj
                {
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = false,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Something went wrong",
                            MessageId = $"CheckOutBookHandler{errorId}",
                            TechnicalMessage = ex.InnerException.Message
                        }
                    }
                };
                #endregion
            }
        }

        #region Build Domain object

        private BooksActivity BuildDomainObject(CheckOutCommand request, ApplicationUser user)
        {
            var checkOut = new BooksActivity
            {
                CustomerId = request.CustomerId,
                BookId = request.BookId,
                CheckOutDate = DateTime.Today,
                Email = user.Email,
                ExpectedReturnDate = request.DateToReturn.Date,
                FullName = user.FullName,
                NationalIdentificationNumber = user.NationalIdentificationNumber ?? "BIMC123QWEG",
                PhoneNumber = user.PhoneNumber,
                ReturnDate = null,
                Status = (int)BookActivityStatus.Check_Out,
                AdminUserId = request.AdminUserId
            };
            return checkOut;
        }

        #endregion


        #region Send Mail method

        private async Task<bool> SendMail(CheckOutCommand request, string bookTitle, DateTime expectedReturnDate, int numberOfdays)
        {
            var users = await _userManager.Users.ToListAsync();
            var customer = users.FirstOrDefault(c => c.Id == request.CustomerId);
            var admin = users.FirstOrDefault(c => c.Id == request.AdminUserId);

            try
            {
                await _emailService.Send(new EmailMessage
                {
                    Content = $" Dear {customer.FullName}. <br> Thanks for your patronage. <br> The book with Title : {bookTitle}" +
                    $" should be returned on the {expectedReturnDate} {numberOfdays} days from now",
                    Subject = "Library Reminder",
                    FromAddresses = new List<EmailAddress>
                        {
                            new EmailAddress{ Address = admin.Email, Name = admin.FullName}
                        },
                    ToAddresses = new List<EmailAddress>
                        {
                            new EmailAddress{ Address = customer.Email, Name = customer.FullName }
                        },
                });
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        #endregion
    }
}
