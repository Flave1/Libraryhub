using AutoMapper;
using Libraryhub.Contracts.Commands;
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
    public class OrderDetailHandler : IRequestHandler<OrderDetailsSearchCommand, OrderDetailResponseObj>
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrderDetailHandler(IOrderService orderService, IMapper mapper)
        {
            _mapper = mapper;
            _orderService = orderService;
        }
        public async Task<OrderDetailResponseObj> Handle(OrderDetailsSearchCommand request, CancellationToken cancellationToken)
        { 
                var orderList = await _orderService.GetAllOrderDetails();
                var searchedOrders = new List<OrderDetail>();
                if (!string.IsNullOrEmpty(request.CustomerId))
                    searchedOrders.AddRange(orderList.Where(x => x.CustomerId == request.CustomerId).ToList());

                if (request.OrderDetailId > 0)
                    searchedOrders.Add(await _orderService.GetOrderDetailById(request.OrderDetailId));

                if (request.OrderId > 0)
                    searchedOrders.AddRange(orderList.Where(x => x.OrderId == request.OrderId).ToList());

                if (request.BookId > 0)
                    searchedOrders.AddRange(orderList.Where(x => x.Order.OrderItems.Select(x => x.BookId == request.BookId).FirstOrDefault()));
                return new OrderDetailResponseObj
                {
                    OrderDetails = _mapper.Map<List<OrderDetailObj>>(searchedOrders),
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = true,
                    }
                }; 
        
        }
    }
}
