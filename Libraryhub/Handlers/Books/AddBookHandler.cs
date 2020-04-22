using AutoMapper;
using Libraryhub.Contracts.Commands;
using Libraryhub.Contracts.RequestObjs;
using Libraryhub.Contracts.Response;
using Libraryhub.CustomError;
using Libraryhub.DomainObjs;
using Libraryhub.ErrorHandler;
using Libraryhub.Service.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Libraryhub.Handlers.Books
{
    public class AddBookHandler : IRequestHandler<AddBookCommand, RegBookResponseObj>
    {
        private readonly IBookService _bookService;
        private readonly ILogger _logger;
        public AddBookHandler(IBookService bookService, ILoggerFactory loggerFactory)
        {
            _bookService = bookService;
            _logger = loggerFactory.CreateLogger(typeof(AddBookHandler));
        }
        public async Task<RegBookResponseObj> Handle(AddBookCommand request, CancellationToken cancellationToken)
        {
            try
            {
                #region Buid book domain object

                var book = new Book
                {
                    CoverPrice = request.CoverPrice,
                    IsAvailable = true,
                    ISBN = request.ISBN,
                    PublishYear = request.PublishYear,
                    Title = request.Title,
                    Quantity = request.Quantity,
                    InitialQuantity = request.Quantity,
                    AccessionNo = request.AccessionNo,
                    Author = request.Author,
                    ClassificationNo = request.ClassificationNo,
                    Color = request.Color,
                    Language = request.Language,
                    Section = request.Section,
                    Size = request.Size
                };

                #endregion

                #region Check if book exist by Title ISBN and pubished year

                var bookExist = await _bookService.BookExistAsync(book);
                if (bookExist)
                {
                    return new RegBookResponseObj
                    {
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = false,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = $"{book.Title} with {book.ISBN} published on {book.PublishYear} already exist"
                            }
                        }
                    };
                }

                #endregion

                #region Add book
                 
                await _bookService.AddNewBookAsync(book);

                return new RegBookResponseObj
                {
                    BookId = book.BookId,
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = true
                    }
                };

                #endregion
            }
            catch (Exception ex)
            {
                #region Log Error with errorId and return error resonse 
                var errorId = ErrorID.Generate(4);
                _logger.LogInformation($"CheckInHandler{errorId}", $"Error Message{ ex.InnerException?.Message ?? ex?.Message}");
                return new RegBookResponseObj
                {
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = false,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = " Unable to process request please contact admin",
                            MessageId = $"CheckInHandler{errorId}",
                            TechnicalMessage = ex.InnerException?.Message ?? ex?.Message
                        }
                    }
                };
                #endregion
            }
        }
    }
}
