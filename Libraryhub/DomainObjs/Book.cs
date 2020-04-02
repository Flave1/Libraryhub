using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Libraryhub.DomainObjs
{
    public class Book
    {
        public int BookId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Book Title is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Title must be between 2 and 100 characters")] 
        public string Title { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Book Title is required")]
        [StringLength(13, MinimumLength = 10, ErrorMessage = "ISBN must be between 10 and 13 characters")]
        public string ISBN { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Date is required")]
        [DataType(DataType.Date)]
        public string PublishYear { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Cover Price is required")]
        public decimal CoverPrice { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Availability is required")]
        public bool IsAvailable { get; set; }
        public virtual List<CheckOutActivity> CheckOutActivities { get; set; }
    }
}
