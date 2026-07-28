using System;
using System.Collections.Generic;
using System.Text;

namespace Tracker.Application.Abstractions
{
    public interface ICurrentUserService
    {
        Guid? UserId { get; } 
    }
}
