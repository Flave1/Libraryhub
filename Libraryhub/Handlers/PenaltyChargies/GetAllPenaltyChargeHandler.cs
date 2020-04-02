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

namespace Libraryhub.Handlers.PenaltyChargies
{
    public class GetAllPenaltyChargeHandler : IRequestHandler<GetAllPenaltyChargeQuery, List<PenaltyChargeResponseObj>>
    {
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;
        public GetAllPenaltyChargeHandler(IBookService bookService, IMapper mapper)
        {
            _bookService = bookService;
            _mapper = mapper;
        }
        public async Task<List<PenaltyChargeResponseObj>> Handle(GetAllPenaltyChargeQuery request, CancellationToken cancellationToken)
        {
            var charges = await _bookService.GetAllPenaltyChargiesAsync();
            return _mapper.Map<List<PenaltyChargeResponseObj>>(charges);
        }
    }
}
