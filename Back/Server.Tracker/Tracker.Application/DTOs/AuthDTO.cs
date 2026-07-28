using System;
using System.Collections.Generic;
using System.Text;

namespace Tracker.Application.DTOs
{
    public record RegisterRequest(
        string NickName,
        string Email,
        string Password
    );

    public record LoginRequest(
        string Email,
        string Password
    );

    public record AuthResponse(
        string Token,
        string NickName,
        string Email
    );

    public record AuthResultDto(
    Guid Id,
    string Email,
    string NickName,
    string Token);

}
