using Libraryhub.Contracts.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Libraryhub.Contracts.RequestObjs
{
    public class AddBookRequestObj : BookSearchObj
    { 
        public decimal CoverPrice { get; set; } 
        public bool IsAvailable { get; set; }
        public int Quantity { get; set; }
    }

    public class EditBookRequestObj : BookSearchObj
    {
        public int BookId { get; set; } 
        public decimal CoverPrice { get; set; }
        public bool IsAvailable { get; set; }
        public int Quantity { get; set; }
    }
    public class BookObj : BookSearchObj
    {
        public int BookId { get; set; } 
        public decimal CoverPrice { get; set; }
        public int Quantity { get; set; }
        public bool IsAvailable { get; set; }
        public IEnumerable<CheckOutActivityObj> CheckOutActivities { get; set; }
    }

   
    public class BookResponseObj
    {
        public IEnumerable<BookObj> Books { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class RegBookResponseObj
    {
        public int BookId { get; set; }
        public APIResponseStatus Status { get; set; }
    }
     
    public class BookSearchObj : IRequest<BookResponseObj>
    {
        public string Title { get; set; }
        public string ISBN { get; set; }
        public bool Status { get; set; }
        public string ClassificationNo { get; set; }
        public string Language { get; set; }
        public string Author { get; set; }
        public string AccessionNo { get; set; }
        public string  Section { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public string PublishYear { get; set; }
    }
}
