using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Tracker.Application.DTOs;

namespace Tracker.Application.Features.Auth.Login.Commands
{
    public record LoginCommand(
        string Email,
        string Password
     ): IRequest<AuthResultDto> ;
}
