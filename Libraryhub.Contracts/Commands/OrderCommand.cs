using Libraryhub.Contracts.RequestObjs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Libraryhub.Contracts.Commands
{

    public class OrderCommand : IRequest<RegOrderResponseObj>
    {
        public string CustomerId { get; set; }
        public bool DeliveredImmediately { get; set; }
        public List<OrderedBooks> Books { get; set; }
    }
    public class OrderDetailsSearchCommand : IRequest<OrderDetailResponseObj>
    {
        public int OrderDetailId { get; set; }
        public string CustomerId { get; set; }
        public int OrderId { get; set; }
        public int OrderStatus { get; set; }
        public int BookId { get; set; }
    }
     
    public class ConfirmOrderCommand : IRequest<RegOrderResponseObj>
    {
        public string CustomerId { get; set; }
        public int OrderId { get; set; }
    }
}
