using AutoMapper;
using Libraryhub.Contracts.Queries;
using Libraryhub.Contracts.RequestObjs;
using Libraryhub.Contracts.Response;
using Libraryhub.ErrorHandler;
using Libraryhub.Service.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Libraryhub.Handlers.Books
{
    public class GetAllBooksHandler : IRequestHandler<GetAllBooksQuery, BookResponseObj>
    {
        private readonly IBookService _bookService;
        private readonly IMapper _mapper; 
        public GetAllBooksHandler(IBookService bookService, IMapper mapper)
        {
            _bookService = bookService;
            _mapper = mapper;
        }

        public async Task<BookResponseObj> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
        {
            var books = await _bookService.GetAllBooksAsync();

            var response = new BookResponseObj
            {
                Books = _mapper.Map<List<BookObj>>(books),
                Status = new APIResponseStatus
                {
                    IsSuccessful = books == null ? true : false,
                    Message = new APIResponseMessage
                    {
                        FriendlyMessage = books == null ? "Search Complete!! No Record Found" : null
                    }
                }
            };
            return response;
        }
    }
}
