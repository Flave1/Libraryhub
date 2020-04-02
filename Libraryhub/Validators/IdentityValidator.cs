using FluentValidation;
using Libraryhub.Contracts.RequestObjs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Libraryhub.Validators
{
    public class UserRegistrationReqObjValidator : AbstractValidator<UserRegistrationReqObj>
    {
        public UserRegistrationReqObjValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8)
                ;
        }
    }
}
