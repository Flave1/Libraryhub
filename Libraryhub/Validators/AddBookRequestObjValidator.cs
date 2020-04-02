using FluentValidation;
using Libraryhub.Contracts.RequestObjs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Libraryhub.Validators
{
    public class AddBookRequestObjValidator : AbstractValidator<AddBookRequestObj>
    {
        public AddBookRequestObjValidator()
        {
            RuleFor(x => x.CoverPrice).NotEmpty();
            RuleFor(x => x.IsAvailable).NotNull();
            RuleFor(x => x.ISBN).NotEmpty().MinimumLength(10).MaximumLength(13);
            RuleFor(x => x.PublishYear).NotEmpty();
            RuleFor(x => x.Title).NotEmpty().MinimumLength(2).MaximumLength(100);
        }
    }
}
