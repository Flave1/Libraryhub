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
    public class GetBookByISBNHandler : IRequestHandler<GetBookByISBNQuery, BookResponseObj>
    {
        private readonly IMapper _mapper;
        private readonly IBookService _bookService;
        public GetBookByISBNHandler(IBookService bookService, IMapper mapper)
        {
            _bookService = bookService;
            _mapper = mapper;
        }

        public async Task<BookResponseObj> Handle(GetBookByISBNQuery request, CancellationToken cancellationToken)
        {
            var post = await _bookService.GetBookByISBNAsync(request.ISBN);
            return post == null ? null : _mapper.Map<BookResponseObj>(post);
        }
    }
}
