using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Libraryhub.Contracts.RequestObjs
{

    public class OrderObj 
    {
        public string CustomerId { get; set; }
        public List<OrderedBooks> Books { get; set; }
    }
    public class OrderedBooks
    {
        public int BookId { get; set; }
        public decimal CoverPrice { get; set; }
        public int Quantity { get; set; }
    }
}
