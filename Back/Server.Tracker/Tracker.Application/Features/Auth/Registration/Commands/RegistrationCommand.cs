using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Tracker.Application.DTOs;
using Tracker.Domain.Entities;

namespace Tracker.Application.Features.Auth.Register.Commands
{
    public record RegistrationCommand(
        string NickName,
        string Email,
        string Password
    ) : IRequest<AuthResultDto>;
}
