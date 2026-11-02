namespace Inventory.Api.Models;

public enum OrderStatus
{
    Placed = 0,
    Completed = 1,
    Cancelled = 2
}

public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public OrderStatus Status { get; set; }
    public List<OrderLine> Lines { get; set; } = [];
    public decimal Total => Lines.Sum(l => l.Quantity * l.UnitPrice);
}

public class OrderLine
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
