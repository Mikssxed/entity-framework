using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace entityframework.entities.configurations
{
    public class StateConfiguration : IEntityTypeConfiguration<State>
    {
        public void Configure(EntityTypeBuilder<State> builder)
        {
            builder.Property(e => e.Message).HasMaxLength(60).IsRequired();
            builder.HasData(
                new State { Id = 1, Message = "To Do" },
                new State { Id = 2, Message = "In Progress" },
                new State { Id = 3, Message = "Done" }
            );
        }
    }
}