using System;
using System.Collections.Generic;
using System.Text;
using Tracker.Domain.Entities;

namespace Tracker.Application.Abstractions
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
        Task<User?> Add(User user, CancellationToken cancellationToken);
    }
}
