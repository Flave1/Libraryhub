using Libraryhub.AppEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Libraryhub.DomainObjs
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public string CustomerId { get; set; }
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Order Order { get; set; }
        public OrderStatus OrderStatus{get;set;}
        public DateTime DateOrdered { get; set; }
        public DateTime? DateDelivered { get; set; }
    }
}
