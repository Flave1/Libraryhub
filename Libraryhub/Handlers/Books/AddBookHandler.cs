using AutoMapper;
using Libraryhub.Contracts.Commands;
using Libraryhub.Contracts.RequestObjs;
using Libraryhub.Contracts.Response;
using Libraryhub.CustomError;
using Libraryhub.DomainObjs;
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
                var book = new Book
                {
                    CoverPrice = request.CoverPrice,
                    IsAvailable = true,
                    ISBN = request.ISBN,
                    PublishYear = request.PublishYear,
                    Title = request.Title, 
                    Quantity = request.Quantity,
                    InitialQuantity = request.Quantity
                };

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

                await _bookService.AddNewBookAsync(book);

                return new RegBookResponseObj
                {
                    BookId = book.BookId,
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = true
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Internal error", ex.InnerException.Message);
                throw new NotImplementedException("Internal error", ex);
            }
        }
    }
}
