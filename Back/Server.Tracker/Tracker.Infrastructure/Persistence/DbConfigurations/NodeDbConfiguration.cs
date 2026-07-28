using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tracker.Domain.Entities;

namespace Tracker.Infrastructure.Persistence.DbConfigurations
{
    public class NodeDbConfiguration: IEntityTypeConfiguration<Node>
    {
        public void Configure(EntityTypeBuilder<Node> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(n => n.Name).HasMaxLength(30).IsRequired();
            builder.Property(x => x.ColorHex).HasMaxLength(30).IsRequired();

            builder.HasMany(t => t.TodoTasks)
                .WithOne(n => n.Node)
                .OnDelete(DeleteBehavior.Cascade);
        }
        
    }
}
