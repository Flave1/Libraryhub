using System;
using System.Collections.Generic;

namespace Libraryhub.Contracts.Response
{
    public class APIResponseStatus
    {
        public bool IsSuccessful { get; set; } = false;
        public string CustomToken;
        public string CustomSetting;
        public APIResponseMessage Message { get; set; }
    }
}
