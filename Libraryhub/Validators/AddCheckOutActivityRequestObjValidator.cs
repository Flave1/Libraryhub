using FluentValidation;
using Libraryhub.Contracts.RequestObjs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Libraryhub.Validators
{
    public class AddCheckOutActivityRequestObjValidator : AbstractValidator<AddCheckOutActivityRequestObj>
    {
        public AddCheckOutActivityRequestObjValidator()
        {
            RuleFor(x => x.BookId).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty(); 
        }
    }
}
