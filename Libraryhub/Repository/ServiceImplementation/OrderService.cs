
using Libraryhub.Data;
using Libraryhub.DomainObjs;
using Libraryhub.Service.Services;
using Microsoft.EntityFrameworkCore;
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

        public async  Task CreateOrderDetails(OrderDetail orderDetail)
        {
          await _dataContext.AddAsync(orderDetail);
        }

        public async Task CreateOrderItemAsync(OrderItem orderItem)
        {
            await _dataContext.AddAsync(orderItem);
        }

        public async Task<IEnumerable<OrderDetail>> GetAllOrderDetails()
        {
            var queryAble = _dataContext.OrderDetails.AsQueryable();
            return await queryAble.Include(x => x.Order).ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            var queryAble = _dataContext.Orders.AsQueryable();
            return await queryAble.Include(x => x.OrderItems).ToListAsync();
        }

        public async Task<Order> GetOrderById(int orderId)
        {
            var queryAble = _dataContext.Orders.AsQueryable();
            return await queryAble.Include(x => x.OrderItems).SingleAsync(x => x.OrderId == orderId);
        }

        public async Task<OrderDetail> GetOrderDetailById(int orderDetaiId)
        {
            var queryAble = _dataContext.OrderDetails.AsQueryable();
            return await queryAble.Include(x => x.Order).SingleOrDefaultAsync(x => x.OrderId == orderDetaiId);
        }

        public async Task<OrderDetail> GetOrderDetailByOrderId(int orderId)
        {
            var queryAble = _dataContext.OrderDetails.AsQueryable();
            return await queryAble.Include(x => x.Order).SingleOrDefaultAsync(x => x.OrderId == orderId);
        }
         
        public async Task SaveChangesAsync()
        {
            await _dataContext.SaveChangesAsync();
        }

        public async Task<bool>  UpdateOrderDetails(OrderDetail orderDetail)
        {
            var Update = await _dataContext.OrderDetails.FindAsync(orderDetail.OrderDetailId);
            _dataContext.Entry(Update).CurrentValues.SetValues(orderDetail);
            return 1 > 0;
        }
    }
}
