using Inventory.Api.Contracts;
using Inventory.Api.Data;
using Inventory.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Api.Services;

public class OrderService(AppDbContext db, IStockNotifier notifier)
{
    public async Task<ServiceResult> PlaceOrderAsync(PlaceOrderRequest request, CancellationToken ct = default)
    {
        var validation = ValidateOrder(request);
        if (validation is not null) return validation;

        var order = new Order { OrderDate = DateTime.UtcNow, Status = OrderStatus.Placed };
        var stockChanges = new List<(int Id, string Name, int Previous, int Current, int Threshold)>();

        foreach (var line in request.Lines)
        {
            var product = await db.Products
                .Where(p => p.Id == line.ProductId)
                .Select(p => new { p.Id, p.Name, p.UnitPrice, p.Stock, p.ReorderThreshold })
                .SingleOrDefaultAsync(ct);

            if (product is null)
                return new ServiceResult.NotFound($"Product {line.ProductId} not found.");

            if (product.Stock < line.Quantity)
                return new ServiceResult.Conflict($"Insufficient stock for product {line.ProductId}.");

            var newStock = product.Stock - line.Quantity;
            await db.Products
                .Where(p => p.Id == line.ProductId)
                .ExecuteUpdateAsync(s => s.SetProperty(p => p.Stock, newStock), ct);

            order.Lines.Add(new OrderLine
            {
                ProductId = product.Id,
                Quantity = line.Quantity,
                UnitPrice = product.UnitPrice
            });

            stockChanges.Add((product.Id, product.Name, product.Stock, newStock, product.ReorderThreshold));
        }

        db.Orders.Add(order);
        await db.SaveChangesAsync(ct);

        foreach (var change in stockChanges)
        {
            await notifier.StockUpdatedAsync(change.Id, change.Current, ct, request.ConnectionId);
            await RaiseLowStockIfCrossedAsync(
                change.Id, change.Name, change.Previous, change.Current, change.Threshold, ct);
        }

        return new ServiceResult.Ok<int>(order.Id);
    }

    public async Task<ServiceResult> RestockAsync(int productId, RestockRequest request, CancellationToken ct = default)
    {
        if (request.Quantity <= 0)
            return new ServiceResult.Invalid("Quantity must be greater than zero.");

        var product = await db.Products
            .Where(p => p.Id == productId)
            .Select(p => new { p.Id, p.Stock })
            .SingleOrDefaultAsync(ct);

        if (product is null)
            return new ServiceResult.NotFound($"Product {productId} not found.");

        await db.Products
            .Where(p => p.Id == productId)
            .ExecuteUpdateAsync(s => s.SetProperty(p => p.Stock, p => p.Stock + request.Quantity), ct);

        await notifier.StockUpdatedAsync(productId, product.Stock + request.Quantity, ct);

        return new ServiceResult.Ok();
    }

    public async Task<ServiceResult> CancelOrderAsync(int orderId, CancellationToken ct = default)
    {
        var order = await db.Orders
            .Include(o => o.Lines)
            .SingleOrDefaultAsync(o => o.Id == orderId, ct);

        if (order is null)
            return new ServiceResult.NotFound($"Order {orderId} not found.");

        if (order.Status == OrderStatus.Cancelled)
            return new ServiceResult.Conflict($"Order {orderId} is already cancelled.");

        order.Status = OrderStatus.Cancelled;

        foreach (var line in order.Lines)
        {
            await db.Products
                .Where(p => p.Id == line.ProductId)
                .ExecuteUpdateAsync(s => s.SetProperty(p => p.Stock, p => p.Stock + line.Quantity), ct);

            var newStock = await db.Products
                .Where(p => p.Id == line.ProductId)
                .Select(p => p.Stock)
                .SingleAsync(ct);

            await notifier.StockUpdatedAsync(line.ProductId, newStock, ct);
        }

        await db.SaveChangesAsync(ct);
        return new ServiceResult.Ok();
    }

    private async Task RaiseLowStockIfCrossedAsync(int productId, string productName, int previousStock, int newStock, int threshold, CancellationToken ct)
    {
        if (previousStock >= threshold && newStock < threshold)
            await notifier.LowStockAlertAsync(productId, productName, newStock, threshold, ct);
    }

    private static ServiceResult? ValidateOrder(PlaceOrderRequest request)
    {
        if (request.Lines.Count == 0)
            return new ServiceResult.Invalid("Order must contain at least one line.");

        if (request.Lines.Any(l => l.Quantity <= 0))
            return new ServiceResult.Invalid("All line quantities must be greater than zero.");

        if (request.Lines.Select(l => l.ProductId).Distinct().Count() != request.Lines.Count)
            return new ServiceResult.Invalid("Duplicate product lines are not allowed.");

        return null;
    }
}
