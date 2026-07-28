using Microsoft.EntityFrameworkCore;
using Tracker.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Tracker.Infrastructure.Persistence.DbConfigurations
{
    public class TodoTaskDbConfiguration: IEntityTypeConfiguration<TodoTask>
    {
        public void Configure(EntityTypeBuilder<TodoTask> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Title).HasMaxLength(60).IsRequired();
            builder.Property(c => c.IsCompleted)
                .HasDefaultValue(false);

            builder.HasOne(u => u.User)
                .WithMany(t => t.TodoTasks)
                .HasForeignKey(u => u.UserId);
            builder.HasOne(n => n.Node)
                .WithMany(t => t.TodoTasks)
                .HasForeignKey(n => n.NodeId);
        }
    }
}
