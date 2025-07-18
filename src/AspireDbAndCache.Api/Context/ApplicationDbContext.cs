using AspireDbAndCache.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace AspireDbAndCache.Api.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<TodoItem> TodoItems { get; set; }
    public DbSet<TodoGroup> TodoGroups { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TodoItem>(entity =>
        {
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<TodoGroup>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasMany(o => o.Items)
                  .WithOne(a => a.Group)
                  .HasForeignKey(o => o.TodoGroupId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
