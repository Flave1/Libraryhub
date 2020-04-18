using Libraryhub.Contracts.RequestObjs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Libraryhub.Contracts.Queries
{
    public class GetAllPenaltyChargeQuery : IRequest<PenaltyChargeResponseObj> { }
    public class GetCustomerPenaltyChargiesQuery : IRequest<PenaltyChargeResponseObj>
    {
        public string CustomerId { get; }
        public GetCustomerPenaltyChargiesQuery(string customerId) { CustomerId = customerId; }
    }
}
