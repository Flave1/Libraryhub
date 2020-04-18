using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Libraryhub.DomainObjs
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int BookId { get; set; }
        public decimal CoverPrice { get; set; }
        public int Quantity { get; set; }
        [ForeignKey("OrderId")]
        public Order Order { get; set; }
    }
}
