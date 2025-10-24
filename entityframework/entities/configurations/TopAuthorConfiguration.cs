using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using entityframework.entities.ViewModels;

namespace entityframework.entities.configurations
{
    public class TopAuthorConfiguration : IEntityTypeConfiguration<TopAuthor>
    {
        public void Configure(EntityTypeBuilder<TopAuthor> builder)
        {
            builder.ToView("View_TopAuthors");
            builder.HasNoKey();
        }
    }
}