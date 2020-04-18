using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Libraryhub.DomainObjs
{
    public class BookPenalty : Admin
    {
        public int BookPenaltyId { get; set; }
        [Required(ErrorMessage = "PenaltyFee is required")]
        public decimal PenaltyFee { get; set; }
        [Required(ErrorMessage = "BookId is required")]
        public int BookId { get; set; }
        [Required(ErrorMessage = "CheckOutActivityId is required")]
        public int CheckOutActivityId { get; set; }
        [Required(ErrorMessage = "Number Of Days Late is required")]
        public long NumberOfDaysLate { get; set; } 
        [Required(ErrorMessage = "Expected Return Date is required")]
        [DataType(DataType.Date)]
        public DateTime ExpectedReturnDate { get; set; } 
        public string CustomerId { get; set; }
    }
}
