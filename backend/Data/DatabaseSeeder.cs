using Inventory.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Api.Data;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(AppDbContext db, CancellationToken ct = default)
    {
        if (await db.Products.AnyAsync(ct)) return;

        if (SupermarketCatalog.Departments.Length != 20
            || SupermarketCatalog.PackVariants.Length != 10
            || SupermarketCatalog.Departments.Any(department => department.Products.Length != 10))
            throw new InvalidOperationException("The supermarket catalog must define 20 departments, 10 products, and 10 pack variants.");

        var categories = SupermarketCatalog.Departments
            .Select(department => new Category { Name = department.Name })
            .ToList();

        db.Categories.AddRange(categories);
        await db.SaveChangesAsync(ct);

        var products = Enumerable.Range(1, 2_000)
            .Select(i =>
            {
                var departmentIndex = (i - 1) % SupermarketCatalog.Departments.Length;
                var itemIndex = (i - 1) / SupermarketCatalog.Departments.Length;
                var department = SupermarketCatalog.Departments[departmentIndex];
                var productName = department.Products[itemIndex % department.Products.Length];
                var variant = SupermarketCatalog.PackVariants[itemIndex / department.Products.Length];

                return new Product
                {
                    Name = string.IsNullOrEmpty(variant) ? productName : $"{productName} - {variant}",
                    Sku = $"{department.Code}-{i:000000}",
                    CategoryId = categories[departmentIndex].Id,
                    UnitPrice = department.BasePrice
                        + (itemIndex % department.Products.Length) * department.PriceStep
                        + (itemIndex / department.Products.Length) * department.PriceStep / 2,
                    Stock = 20 + i % 80,
                    ReorderThreshold = 15
                };
            })
            .ToList();

        if (products.Select(product => product.Name).Distinct().Count() != products.Count)
            throw new InvalidOperationException("Supermarket product names must be unique.");

        db.Products.AddRange(products);
        await db.SaveChangesAsync(ct);

        var orderCount = int.TryParse(Environment.GetEnvironmentVariable("SEED_ORDER_COUNT"), out var configured)
            ? Math.Max(configured, 0)
            : 250_000;

        if (orderCount == 0) return;

        await db.Database.ExecuteSqlInterpolatedAsync($"""
            SET NOCOUNT ON;
            WITH numbers AS (
                SELECT TOP ({orderCount}) ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS n
                FROM sys.all_objects a CROSS JOIN sys.all_objects b
            )
            INSERT INTO Orders (OrderDate, Status)
            SELECT DATEADD(MINUTE, -n, SYSUTCDATETIME()), n % 3
            FROM numbers;
            """, ct);
    }
}
