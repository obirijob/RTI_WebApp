using Inventory.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderLine> OrderLines => Set<OrderLine>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(e =>
        {
            e.Property(p => p.Sku).HasMaxLength(32);
            e.Property(p => p.Name).HasMaxLength(200);
            e.Property(p => p.UnitPrice).HasPrecision(18, 2);
            e.Property(p => p.RowVersion).IsRowVersion();
            e.HasIndex(p => p.Sku).IsUnique();
        });

        modelBuilder.Entity<Category>(e =>
        {
            e.Property(c => c.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Order>(e =>
        {
            e.HasIndex(o => new { o.Status, o.OrderDate })
                .IsDescending(false, true)
                .HasDatabaseName("IX_Orders_Status_OrderDate");
        });

        modelBuilder.Entity<OrderLine>(e =>
        {
            e.Property(l => l.UnitPrice).HasPrecision(18, 2);
        });
    }
}
