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
        public DbSet<User> Users { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Address> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WorkItem>(eb =>
            {
                eb.Property(e => e.IterationPath).HasColumnName("Iteration_Path");
                eb.Property(e => e.Effort).HasColumnType("decimal(5,2)");
                eb.Property(e => e.RemainingWork).HasPrecision(3);
                eb.Property(e => e.Activity).HasMaxLength(200);
                eb.Property(e => e.RemainingWork).HasPrecision(14, 2);
                eb.Property(x => x.State).IsRequired();
                eb.Property(x => x.Area).HasColumnType("varchar(200)");
                eb.Property(x => x.Priority).HasDefaultValue(1);
            });

            modelBuilder.Entity<Comment>(eb =>
            {
                eb.Property(e => e.CreatedDate).HasDefaultValueSql("getutcdate()");
                eb.Property(e => e.UpdatedDate).ValueGeneratedOnUpdate();
            });
        }
    }
}