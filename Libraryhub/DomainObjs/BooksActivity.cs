using Libraryhub.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Libraryhub.DomainObjs
{
    public class BooksActivity : Admin
    { 
        
        [Key]
        public int CheckOutActivityId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Book Id is required")]
        public int BookId { get; set; }
        public DateTime? ReturnDate { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Expected Return Date is required")]
        [DataType(DataType.Date)] 
        public DateTime ExpectedReturnDate { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Check out Date is required")]
        [DataType(DataType.Date)]
        public DateTime CheckOutDate { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Book Status required")]
        public int Status {get;set;}

        [Required(AllowEmptyStrings = false, ErrorMessage = "CustomerId is required")]
        public string CustomerId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email required")]
        [EmailAddress]
        public string  Email { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "FullName is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "FullName must be between 2 and 50 characters")]
        public string FullName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Phone Number is required")]
        [StringLength(12, MinimumLength = 2, ErrorMessage = "Phone Number must be between 2 and 12 characters")]
        public string PhoneNumber { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "National Identification Number is required")]
        [StringLength(12, MinimumLength = 2, ErrorMessage = "National Identification Number must be between 2 and 12 characters")]
        public string NationalIdentificationNumber { get; set; }

    }
}
