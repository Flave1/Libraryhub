using System;
using System.Collections.Generic;
using System.Text;

namespace Libraryhub.Contracts.Response
{


    public class APIResponseMessage
    {
        public string FriendlyMessage { get; set; }
        public string TechnicalMessage { get; set; }
        public string MessageId { get; set; }
        public string SearchResultMessage;
        public string ShortErrorMessage; 
    }
}
