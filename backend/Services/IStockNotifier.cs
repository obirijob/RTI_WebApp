using Inventory.Api.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Inventory.Api.Services;

public interface IStockNotifier
{
    Task StockUpdatedAsync(int productId, int newStock, CancellationToken ct = default, string? requesterConnectionId = null);
    Task LowStockAlertAsync(int productId, string productName, int stock, int reorderThreshold, CancellationToken ct = default);
}

public class SignalRStockNotifier(IHubContext<StockHub> hub) : IStockNotifier
{
    public const string StockWatchersGroup = "stock-watchers";

    public Task StockUpdatedAsync(int productId, int newStock, CancellationToken ct = default, string? requesterConnectionId = null)
    {
        var clients = requesterConnectionId is null
            ? hub.Clients.Group(StockWatchersGroup)
            : hub.Clients.Client(requesterConnectionId);

        return clients.SendAsync("StockUpdated", productId, newStock, ct);
    }

    public Task LowStockAlertAsync(int productId, string productName, int stock, int reorderThreshold, CancellationToken ct = default)
        => hub.Clients.Group(StockWatchersGroup)
            .SendAsync("LowStockAlert", new { productId, productName, stock, reorderThreshold }, ct);
}
