using Libraryhub.Contracts.RequestObjs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Libraryhub.Contracts.Commands
{
    public class AddBookCommand : IRequest<BookResponseObj>
    { 
        public string Title { get; set; }
        public string ISBN { get; set; }
        public string PublishYear { get; set; }
        public decimal CoverPrice { get; set; }
        public bool IsAvailable { get; set; }
    }

    public class EditBookCommand : IRequest<BookResponseObj>
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public string PublishYear { get; set; }
        public decimal CoverPrice { get; set; }
        public bool IsAvailable { get; set; }
    }

    public class CheckOutCommand : IRequest<CheckOutActivityResponseObj>
    {
        public int BookId { get; set; } 
        public string UserId { get; set; }
    }

    public class CheckInCommand : IRequest<CheckOutActivityResponseObj>
    {
        public int CheckOutActivityId { get; set; }
        public int BookId { get; set; }
        public string UserId { get; set; }
        public DateTime ReturnDate { get; set; }
        public decimal? PenaltyFee { get; set; }
    }
}
