using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Tracker.Application.Abstractions;
using Tracker.Domain.Entities;

namespace Tracker.Infrastructure.Persistence
{
    public class TrackerDbContext: DbContext, ITrackerDbContext
    {
        public TrackerDbContext(DbContextOptions<TrackerDbContext> options) : base(options) { }

        public DbSet<Node> Nodes => Set<Node>();
        public DbSet<TodoTask> TodoTasks => Set<TodoTask>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // данные Node сделаны как базовый 
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Node>().HasData(
                new Node { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "HOME", ColorHex = "#00ffcc" },
                new Node { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "WORK", ColorHex = "#3b82f6" },
                new Node { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), Name = "STUDY", ColorHex = "#a855f7" },
                new Node { Id = Guid.Parse("44444444-4444-4444-4444-444444444444"), Name = "HEALTH", ColorHex = "#ec4899" },
                new Node { Id = Guid.Parse("55555555-5555-5555-5555-555555555555"), Name = "FINANCE", ColorHex = "#eab308" },
                new Node { Id = Guid.Parse("66666666-6666-6666-6666-666666666666"), Name = "PERSONAL", ColorHex = "#6b7280" },
                new Node { Id = Guid.Parse("77777777-7777-7777-7777-777777777777"), Name = "CREATIVE", ColorHex = "#10b981" },
                new Node { Id = Guid.Parse("88888888-8888-8888-8888-888888888888"), Name = "SOCIAL", ColorHex = "#f97316" }
            );
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
