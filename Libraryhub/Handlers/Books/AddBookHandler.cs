using AutoMapper;
using Libraryhub.Contracts.Commands;
using Libraryhub.Contracts.RequestObjs;
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
    public class AddBookHandler : IRequestHandler<AddBookCommand, BookResponseObj>
    {
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public AddBookHandler(IBookService bookService, IMapper mapper, ILoggerFactory loggerFactory)
        {
            _bookService = bookService;
            _mapper = mapper;
            _logger = loggerFactory.CreateLogger(typeof(AddBookHandler));
        }
        public async Task<BookResponseObj> Handle(AddBookCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var book = new Book
                {
                    CoverPrice = request.CoverPrice,
                    IsAvailable = request.IsAvailable,
                    ISBN = request.ISBN,
                    PublishYear = request.PublishYear,
                    Title = request.Title, 
                };
                var result = await _bookService.AddNewBookAsync(book);
                var response = _mapper.Map<BookResponseObj>(book);
                return await Task.Run(() => response);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Internal error", ex.InnerException.Message);
                throw new NotImplementedException("Internal error", ex);
            }
        }
    }
}
