using System;
using System.Collections.Generic;
using System.Text;
using Tracker.Domain.Entities;


namespace Tracker.Application.Abstractions
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);
    }
}
