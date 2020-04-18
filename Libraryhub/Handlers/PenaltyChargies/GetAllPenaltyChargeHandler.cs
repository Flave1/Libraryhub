using AutoMapper;
using Libraryhub.Contracts.Queries;
using Libraryhub.Contracts.RequestObjs;
using Libraryhub.Contracts.Response;
using Libraryhub.Service.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Libraryhub.Handlers.PenaltyChargies
{
    public class GetAllPenaltyChargeHandler : IRequestHandler<GetAllPenaltyChargeQuery, PenaltyChargeResponseObj>
    {
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;
        public GetAllPenaltyChargeHandler(IBookService bookService, IMapper mapper)
        {
            _bookService = bookService;
            _mapper = mapper;
        }
        public async Task<PenaltyChargeResponseObj> Handle(GetAllPenaltyChargeQuery request, CancellationToken cancellationToken)
        {
            var penalties = await _bookService.GetAllPenaltyChargiesAsync();

            var response = new PenaltyChargeResponseObj
            {
                PenaltyCharges = _mapper.Map<List<BookPenaltyObj>>(penalties),
                Status = new APIResponseStatus
                {
                    IsSuccessful = penalties == null ? true : false,
                    Message = new APIResponseMessage
                    {
                        FriendlyMessage = penalties == null ? "Search Complete!! No Record Found" : null
                    }
                }
            };
            return response;
        }
    }
}
