using Libraryhub.Contracts.RequestObjs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Libraryhub.Contracts.Commands
{
    public class AddBookCommand : IRequest<RegBookResponseObj>
    { 
        public string Title { get; set; }
        public string ISBN { get; set; }
        public string PublishYear { get; set; }
        public decimal CoverPrice { get; set; }
        public bool IsAvailable { get; set; }
        public int Quantity { get; set; }

        public string ClassificationNo { get; set; }
        public string Language { get; set; }
        public string Author { get; set; }
        public string AccessionNo { get; set; }
        public string Section { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
    }

    public class EditBookCommand : IRequest<RegBookResponseObj>
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public string PublishYear { get; set; }
        public decimal CoverPrice { get; set; }
        public bool IsAvailable { get; set; }
        public int Quantity { get; set; }

        public string ClassificationNo { get; set; }
        public string Language { get; set; }
        public string Author { get; set; }
        public string AccessionNo { get; set; }
        public string Section { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
    }

    public class CheckOutCommand : IRequest<CheckOutActivityResponseObj>
    {
        public int BookId { get; set; } 
        public string CustomerId { get; set; }
        public string AdminUserId { get; set; }
        public DateTime DateToReturn { get; set; }
    }

    public class CheckInCommand : IRequest<CheckOutActivityResponseObj>
    {
        public int CheckOutActivityId { get; set; }
        public int BookId { get; set; }
        public string CustomerId { get; set; }
        public string AdminUserId { get; set; }
        public DateTime ReturnDate { get; set; }
        public decimal? PenaltyFee { get; set; }
    }

    


}
