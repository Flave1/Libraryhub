using FluentValidation;
using Libraryhub.Contracts.RequestObjs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Libraryhub.Validators
{
    public class EditCheckOutActivityRequestObjValidator : AbstractValidator<EditCheckOutActivityRequestObj>
    {
        public EditCheckOutActivityRequestObjValidator()
        {
            RuleFor(x => x.BookId).NotEmpty();
            RuleFor(x => x.CustomerId).NotEmpty();
            RuleFor(x => x.AdminUserId).NotEmpty();
            RuleFor(x => x.CheckOutActivityId).NotEmpty();
        }
    }
}
