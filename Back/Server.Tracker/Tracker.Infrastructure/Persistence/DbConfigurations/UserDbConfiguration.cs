using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tracker.Domain.Entities;

namespace Tracker.Infrastructure.Persistence.DbConfigurations
{
    public class UserDbConfiguration: IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(n => n.NickName).HasMaxLength(50).IsRequired();
            builder.Property(e => e.Email).HasMaxLength(60).IsRequired();
            builder.Property(p => p.PasswordHash).HasMaxLength(60).IsRequired();

            builder.HasMany(t => t.TodoTasks)
                .WithOne(u => u.User)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
