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
    public class GetBookByStatusHandler : IRequestHandler<GetBookByStatusQuery, List<BookResponseObj>>
    {
        private readonly IMapper _mapper;
        private readonly IBookService _bookService;
        public GetBookByStatusHandler(IBookService bookService, IMapper mapper)
        {
            _bookService = bookService;
            _mapper = mapper;
        }
        public async Task<List<BookResponseObj>> Handle(GetBookByStatusQuery request, CancellationToken cancellationToken)
        {
            var post = await _bookService.GetBookByStatusAsync(request.Status);
            return post == null ? null : _mapper.Map<List<BookResponseObj>>(post);
        }
    }
}
