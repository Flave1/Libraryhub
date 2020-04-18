using System;
using System.Collections.Generic;
using System.Text;

namespace Libraryhub.Contracts.RequestObjs
{
    public class BookActivityObj
    {
        public int BookActivityId { get; set; }
        public int BookId { get; set; }
        public string CustomerId { get; set; }
        public DateTime ReturnDate { get; set; }
        public DateTime ExpectedReturnDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int Status { get; set; }
    }
}
