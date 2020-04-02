using Libraryhub.Contracts.RequestObjs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Libraryhub.Contracts.Queries
{
    public class GetAllPenaltyChargeQuery : IRequest<List<PenaltyChargeResponseObj>> { }
    public class GetCustomerPenaltyChargiesQuery : IRequest<List<PenaltyChargeResponseObj>>
    {
        public string UserId { get; }
        public GetCustomerPenaltyChargiesQuery(string userId) { UserId = userId; }
    }
}
