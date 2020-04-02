using Libraryhub.Contracts.ErrorResponses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Libraryhub.Contracts.RequestObjs
{

    public class AddCheckOutActivityRequestObj
    { 
        public int BookId { get; set; } 
        public string UserId { get; set; }

    }
    public class EditCheckOutActivityRequestObj
    {
        public int CheckOutActivityId { get; set; }
        public int BookId { get; set; }
        public string UserId { get; set; }
        public DateTime ReturnDate { get; set; }
        public decimal? PenaltyFee { get; set; }

    }
    public class CheckOutActivityObj
    {
        public int CheckOutActivityId { get; set; } 
        public int BookId { get; set; }
        public DateTime? ReturnDate { get; set; } 
        public DateTime ExpectedReturnDate { get; set; } 
        public DateTime CheckOutDate { get; set; } 
        public int Status { get; set; } 
        public string UserId { get; set; } 
        public string Email { get; set; } 
        public string FullName { get; set; } 
        public string PhoneNumber { get; set; } 
        public string NationalIdentificationNumber { get; set; }
    }
    public class CheckOutActivityResponseObj
    {
        public int CheckOutActivityId { get; set; }
        public int BookId { get; set; }
        public DateTime? ReturnDate { get; set; }
        public DateTime ExpectedReturnDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int Status { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string NationalIdentificationNumber { get; set; }
        public UserMessage UserMessage { get; set; }
    }
}
