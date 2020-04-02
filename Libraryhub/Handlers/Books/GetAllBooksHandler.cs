using AutoMapper;
using Libraryhub.Contracts.Queries;
using Libraryhub.Contracts.RequestObjs;
using Libraryhub.Service.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Libraryhub.Handlers.Books
{
    public class GetAllBooksHandler : IRequestHandler<GetAllBooksQuery, List<BookResponseObj>>
    {
        private readonly IBookService _bookService;
        private readonly IMapper _mapper; 
        public GetAllBooksHandler(IBookService bookService, IMapper mapper)
        {
            _bookService = bookService;
            _mapper = mapper;
        } 

        async Task<List<BookResponseObj>> IRequestHandler<GetAllBooksQuery, List<BookResponseObj>>.Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
        {
            var books = await _bookService.GetAllBooksAsync();
            return _mapper.Map<List<BookResponseObj>>(books);
        }
    }
}
