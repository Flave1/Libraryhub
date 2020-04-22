using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Libraryhub.DomainObjs
{
    public class Order 
    {
        public int OrderId { get; set; }
        public string CustomerId { get; set; }
        public int TotalQuantity { get; set; }
        public decimal GrandPrice { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}
