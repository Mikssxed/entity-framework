using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace entityframework.entities.configurations
{
    public class TaskConfiguration : IEntityTypeConfiguration<Task>
    {
        public void Configure(EntityTypeBuilder<Task> builder)
        {
            builder.Property(e => e.RemainingWork).HasPrecision(14, 2);
            builder.Property(e => e.Activity).HasMaxLength(200);
        }
    }
}