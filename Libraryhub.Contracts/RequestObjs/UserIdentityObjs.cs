using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Libraryhub.Contracts.RequestObjs
{
    public class UserRegistrationReqObj
    {
        [EmailAddress]
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string NationalIdentificationNumber { get; set; }

    }

    public class UserLoginReqObj
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UserRefreshTokenReqObj
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
    public class AuthFailedResponse
    {
        public IEnumerable<string> Errors { get; set; }
    }

    public class AuthSuccessResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
