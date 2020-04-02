using Libraryhub.Contracts.ErrorResponses;
using System;
using System.Collections.Generic;

namespace flavehub.Contracts.ErrorResponses
{
    public class ErrorResponse
    {
        public List<ErrorModel> Errors { get; set; } = new List<ErrorModel>();
         
    }
}
