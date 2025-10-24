using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace entityframework.entities.configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.Property(e => e.CreatedDate).HasDefaultValueSql("getutcdate()");
            builder.Property(e => e.UpdatedDate).ValueGeneratedOnUpdate();
            builder.HasOne(c => c.User).WithMany(u => u.Comments).HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}