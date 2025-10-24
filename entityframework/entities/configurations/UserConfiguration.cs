using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace entityframework.entities.configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasOne(u => u.Address).WithOne(a => a.User).HasForeignKey<Address>(a => a.UserId);
            builder.HasIndex(u => new { u.Email, u.FullName });
        }
    }
}