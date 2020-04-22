using Libraryhub.Data;
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
        Task CreateOrderItemAsync(OrderItem orderItem);
        Task CreateOrderDetails(OrderDetail orderDetail);
        Task<bool> UpdateOrderDetails(OrderDetail orderDetail);

        Task<IEnumerable<Order>> GetAllOrders();
        Task<Order> GetOrderById(int orderId);
        Task<OrderDetail> GetOrderDetailByOrderId(int orderId);
        Task<IEnumerable<OrderDetail>> GetAllOrderDetails();
        Task<OrderDetail> GetOrderDetailById(int orderDetaiId);


        Task SaveChangesAsync();
    }
}
