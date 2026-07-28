using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tracker.Application.Features.Auth.Login.Commands
{
    public class LoginCommandValidator: AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(e => e.Email)
                .NotEmpty().WithMessage("Write Email")
                .EmailAddress().WithMessage("Uncorrect Email");

            RuleFor(p => p.Password)
                .NotEmpty().WithMessage("Write Password");
        }
    }
}
