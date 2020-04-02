using System;
using System.Collections.Generic;
using System.Text;

namespace Libraryhub.Contracts.RequestObjs
{
    public class AddBookRequestObj
    { 
        public string Title { get; set; } 
        public string ISBN { get; set; } 
        public string PublishYear { get; set; } 
        public decimal CoverPrice { get; set; } 
        public bool IsAvailable { get; set; }
       
    }

    public class EditBookRequestObj
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public string PublishYear { get; set; }
        public decimal CoverPrice { get; set; }
        public bool IsAvailable { get; set; }
    }
    public class BookObj
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public string PublishYear { get; set; }
        public decimal CoverPrice { get; set; }
        public bool IsAvailable { get; set; }
    }

    public class BookResponseObj
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public string PublishYear { get; set; }
        public decimal CoverPrice { get; set; }
        public bool IsAvailable { get; set; }
        public IEnumerable<CheckOutActivityObj> CheckOutActivities { get; set; }
    }

    public class BookByTitleSearchObj
    {
        public string Title { get; set; }
    }
    public class BookByISBNSearchObj
    {
        public string ISBN { get; set; }
    }

    public class BookByStatusSearchObj
    {
        public bool Status { get; set; }
    }
}
