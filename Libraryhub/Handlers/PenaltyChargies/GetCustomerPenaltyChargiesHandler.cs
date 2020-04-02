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
    public class GetCustomerPenaltyChargiesHandler : IRequestHandler<GetCustomerPenaltyChargiesQuery, List<PenaltyChargeResponseObj>>
    {
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;
        public GetCustomerPenaltyChargiesHandler(IBookService bookService, IMapper mapper)
        {
            _bookService = bookService;
            _mapper = mapper;
        }
        public async Task<List<PenaltyChargeResponseObj>> Handle(GetCustomerPenaltyChargiesQuery request, CancellationToken cancellationToken)
        {
            var charges = await _bookService.GetPenaltyChargiesForCustomer(request.UserId);
            return _mapper.Map<List<PenaltyChargeResponseObj>>(charges);
        }
    }
}
