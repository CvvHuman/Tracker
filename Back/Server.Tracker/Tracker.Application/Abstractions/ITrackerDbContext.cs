using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tracker.Domain.Entities;

namespace Tracker.Application.Abstractions
{
    public interface ITrackerDbContext
    {
        DbSet<User> Users { get; }
        DbSet<Node> Nodes { get; }
        DbSet<TodoTask> TodoTasks { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
