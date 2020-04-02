using System;
using System.Collections.Generic;
using System.Text;

namespace Libraryhub.Contracts.RequestObjs
{
    public class PenaltyChargeResponseObj
    {
        public int BookPenaltyId { get; set; } 
        public decimal PenaltyFee { get; set; } 
        public int BookId { get; set; } 
        public int CheckOutActivityId { get; set; } 
        public long NumberOfDaysLate { get; set; }  
        public DateTime ExpectedReturnDate { get; set; }
        public string UserId { get; set; }
    }
    public class CustomerPenaltyChargeSearchObj
    {
        public string UserId { get; set; }
    }
}
