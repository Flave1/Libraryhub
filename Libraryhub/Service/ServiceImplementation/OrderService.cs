
using Libraryhub.Data;
using Libraryhub.DomainObjs;
using Libraryhub.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Libraryhub.Service.ServiceImplementation
{
    public class OrderService : IOrderService
    {
        private readonly DataContext _dataContext;
        public OrderService(DataContext dataContext)
        {
            _dataContext = dataContext; 
        }
        public  void CreateOrder(Order order)
        {
            _dataContext.Add(order);
        }

        public void CreateOrderItem(OrderItem orderItem)
        {
            _dataContext.Add(orderItem);
        }
    }
}
