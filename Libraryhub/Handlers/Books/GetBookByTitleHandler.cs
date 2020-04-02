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
    public class GetBookByTitleHandler : IRequestHandler<GetBookByTitleQuery, BookResponseObj>
    {
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;
        public GetBookByTitleHandler(IBookService bookService, IMapper mapper)
        {
            _mapper = mapper;
            _bookService = bookService;
        }

        public async Task<BookResponseObj> Handle(GetBookByTitleQuery request, CancellationToken cancellationToken)
        {
            var post = await _bookService.GetBookByTitleAsync(request.Title);
            return post == null ? null : _mapper.Map<BookResponseObj>(post);
        }
    }
}
