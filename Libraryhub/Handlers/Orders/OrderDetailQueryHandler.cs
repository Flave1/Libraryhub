using AutoMapper;
using Libraryhub.Contracts.Queries;
using Libraryhub.Contracts.RequestObjs;
using Libraryhub.Contracts.Response;
using Libraryhub.DomainObjs;
using Libraryhub.Service.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Libraryhub.Handlers.Orders
{
    public class OrderDetailQueryHandler : IRequestHandler<OrderDetailQuery, OrderDetailResponseObj>
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        public OrderDetailQueryHandler(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }
        public async Task<OrderDetailResponseObj> Handle(OrderDetailQuery request, CancellationToken cancellationToken)
        {
            var details = await _orderService.GetAllOrderDetails();
            return new OrderDetailResponseObj
            {
                OrderDetails = details != null? _mapper.Map<List<OrderDetailObj>>(details): null,
                Status = new APIResponseStatus
                {
                    IsSuccessful = details != null ? true: false,
                }
            };
        }
    }
}
