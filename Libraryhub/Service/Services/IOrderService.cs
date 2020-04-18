using Libraryhub.DomainObjs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Libraryhub.Service.Services
{
    public interface IOrderService
    {
        void CreateOrder(Order order);
        void CreateOrderItem(OrderItem orderItem);
    }
}
