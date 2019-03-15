using Application.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Validators
{
    public class RequestPasswordValidator : AbstractValidator<RequestPasswordCommand>
    {
        public RequestPasswordValidator()
        {
            RuleFor(x => x.UserName).NotNull();
            RuleFor(x => x.Password).NotNull();
            RuleFor(x => x.SecretCode).NotNull();
        }
    }
}
