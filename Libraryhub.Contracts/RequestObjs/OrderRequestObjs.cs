using Libraryhub.Contracts.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Libraryhub.Contracts.RequestObjs
{

    public class OrderObj 
    {
        public string CustomerId { get; set; }
        public bool DeliveredImmediately { get; set; }
        public List<OrderedBooks> Books { get; set; }
    }
    public class OrderedBooks
    {
        public int BookId { get; set; } 
        public int Quantity { get; set; }
    }

    public class OrderDetailsSearch
    {
        public int OrderDetailId { get; set; }
        public string CustomerId { get; set; }
        public int OrderId { get; set; } 
        public int OrderStatus { get; set; }
        public int BookId { get; set; }
    }

    public class OrderDetailObj
    {
        public int OrderDetailId { get; set; }
        public string CustomerId { get; set; }
        public int OrderId { get; set; } 
        public OrderObj Order { get; set; } 
    }
     
    public class ConfirmOrder
    {
        public string CustomerId { get; set; }
        public int OrderId { get; set; }
    }

    public class RegOrderResponseObj
    {
        public int OrderId { get; set; }
        public APIResponseStatus Status { get; set; } 
    }

    public class OrderResponseObj
    { 
        public APIResponseStatus Status { get; set; }
        public List<OrderObj> Orders { get; set; }
    }

    public class OrderDetailResponseObj
    {
        public APIResponseStatus Status { get; set; }
        public List<OrderDetailObj> OrderDetails { get; set; }
    }
}
