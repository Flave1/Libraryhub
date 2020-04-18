using Libraryhub.Contracts.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Libraryhub.Contracts.RequestObjs
{
    public class EmailResponseObj
    {
        public bool IsSuccessful { get; set; }
        public APIResponseStatus Status { get; set; }
    }

}
