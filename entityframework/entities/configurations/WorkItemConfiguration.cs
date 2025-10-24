using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace entityframework.entities.configurations
{
    public class WorkItemConfiguration : IEntityTypeConfiguration<WorkItem>
    {
        public void Configure(EntityTypeBuilder<WorkItem> builder)
        {
            builder.Property(e => e.IterationPath).HasColumnName("Iteration_Path");
            builder.Property(x => x.Area).HasColumnType("varchar(200)");
            builder.Property(x => x.Priority).HasDefaultValue(1);

            builder.HasMany(w => w.Comments).WithOne(c => c.WorkItem).HasForeignKey(c => c.WorkItemId);
            builder.HasOne(w => w.Author).WithMany(u => u.WorkItems).HasForeignKey(w => w.AuthorId);
            builder.HasMany(w => w.Tags).WithMany(t => t.WorkItems).UsingEntity<WorkItemTag>(
                w => w.HasOne(wit => wit.Tag).WithMany().HasForeignKey(wit => wit.TagId),
                w => w.HasOne(wit => wit.WorkItem).WithMany().HasForeignKey(wit => wit.WorkItemId),
                w =>
                {
                    w.HasKey(x => new { x.TagId, x.WorkItemId });
                    w.Property(x => x.PublicationDate).HasDefaultValueSql("getutcdate()");
                }
            );

            builder.HasOne(w => w.State).WithMany().HasForeignKey(w => w.StateId);
        }
    }
}