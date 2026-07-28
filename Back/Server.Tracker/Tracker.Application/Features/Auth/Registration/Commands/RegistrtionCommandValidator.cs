using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using Tracker.Application.Features.Auth.Login.Commands;
using Tracker.Application.Features.Auth.Register.Commands;

namespace Tracker.Application.Features.Auth.Registration.Commands
{
    public class RegistrtionCommandValidator : AbstractValidator<RegistrationCommand>
    {
        public RegistrtionCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email не может быть пустым.")
                .EmailAddress().WithMessage("Некорректный формат Email адреса.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Пароль не может быть пустым.")
                .MinimumLength(6).WithMessage("Пароль должен содержать минимум 6 символов.")
                .Matches(@"[A-Z]").WithMessage("Пароль должен содержать хотя бы одну заглавную букву.")
                .Matches(@"[a-z]").WithMessage("Пароль должен содержать хотя бы одну строчную букву.")
                .Matches(@"[0-9]").WithMessage("Пароль должен содержать хотя бы одну цифру.");

            RuleFor(x => x.NickName)
                .NotEmpty().WithMessage("Имя обязательно для заполнения.")
                .MaximumLength(50).WithMessage("Имя не должно превышать 50 символов.");
        }
    }
}
