namespace Inventory.Api.Models;

public class Product
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Sku { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    public decimal UnitPrice { get; set; }
    public int Stock { get; set; }
    public int ReorderThreshold { get; set; }
    public byte[] RowVersion { get; set; } = [];
}
