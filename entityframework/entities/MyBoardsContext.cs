using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace entityframework.entities
{
    public class MyBoardsContext : DbContext
    {
        public MyBoardsContext(DbContextOptions<MyBoardsContext> options) : base(options)
        {

        }
        public DbSet<WorkItem> WorkItems { get; set; }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Epic> Epics { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Address> Addresses { get; set; }

        public DbSet<State> States { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Epic>().Property(e => e.EndDate).HasPrecision(3);
            modelBuilder.Entity<Task>().Property(e => e.RemainingWork).HasPrecision(14, 2);
            modelBuilder.Entity<Task>().Property(e => e.Activity).HasMaxLength(200);
            modelBuilder.Entity<Issue>().Property(e => e.Effort).HasColumnType("decimal(5,2)");

            modelBuilder.Entity<WorkItem>(eb =>
            {
                eb.Property(e => e.IterationPath).HasColumnName("Iteration_Path");
                eb.Property(x => x.Area).HasColumnType("varchar(200)");
                eb.Property(x => x.Priority).HasDefaultValue(1);
                eb.HasMany(w => w.Comments).WithOne(c => c.WorkItem).HasForeignKey(c => c.WorkItemId);
                eb.HasOne(w => w.Author).WithMany(u => u.WorkItems).HasForeignKey(w => w.AuthorId);
                eb.HasMany(w => w.Tags).WithMany(t => t.WorkItems).UsingEntity<WorkItemTag>(
                    w => w.HasOne(wit => wit.Tag).WithMany().HasForeignKey(wit => wit.TagId),

                    w => w.HasOne(wit => wit.WorkItem).WithMany().HasForeignKey(wit => wit.WorkItemId),

                    w =>
                    {
                        w.HasKey(x => new { x.TagId, x.WorkItemId });
                        w.Property(x => x.PublicationDate).HasDefaultValueSql("getutcdate()");
                    }
                );

                eb.HasOne(w => w.State).WithMany().HasForeignKey(w => w.StateId);
            });

            modelBuilder.Entity<Comment>(eb =>
            {
                eb.Property(e => e.CreatedDate).HasDefaultValueSql("getutcdate()");
                eb.Property(e => e.UpdatedDate).ValueGeneratedOnUpdate();
                eb.HasOne(c => c.User).WithMany(u => u.Comments).HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<User>().HasOne(u => u.Address).WithOne(a => a.User).HasForeignKey<Address>(a => a.UserId);
            modelBuilder.Entity<State>(eb =>
            {
                eb.Property(e => e.Message).HasMaxLength(60).IsRequired();
                eb.HasData(
                    new State { Id = 1, Message = "To Do" },
                    new State { Id = 2, Message = "In Progress" },
                    new State { Id = 3, Message = "Done" }
                );
            });
        }
    }
}