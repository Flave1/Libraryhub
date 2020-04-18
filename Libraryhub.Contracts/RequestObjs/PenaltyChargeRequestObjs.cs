using Libraryhub.Contracts.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Libraryhub.Contracts.RequestObjs
{
    public class BookPenaltyObj
    {
        public int BookPenaltyId { get; set; } 
        public decimal PenaltyFee { get; set; } 
        public int BookId { get; set; } 
        public int CheckOutActivityId { get; set; } 
        public long NumberOfDaysLate { get; set; }  
        public DateTime ExpectedReturnDate { get; set; }
        public string CustomerId { get; set; }
        public string AdminUserId { get; set; }
    }
    public class PenaltyChargeResponseObj
    {
        public IEnumerable<BookPenaltyObj> PenaltyCharges { get; set; }
        public APIResponseStatus Status { get; set; }
    }
    public class CustomerPenaltyChargeSearchObj
    {
        public string CustomerId { get; set; }
    }
}
