using AspireDbAndCache.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace AspireDbAndCache.Api.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Expense> Expenses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Category>()
            .ToTable("Categories")
            .HasKey(c => c.Id);
        modelBuilder.Entity<Category>()
            .HasMany(x => x.Expenses).WithOne(x => x.Category).HasForeignKey(x => x.CategoryId);

        modelBuilder.Entity<Expense>()
            .ToTable("Expenses")
            .HasKey(e => e.Id);
    }
}
