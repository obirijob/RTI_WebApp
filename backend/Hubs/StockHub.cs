using Inventory.Api.Services;
using Microsoft.AspNetCore.SignalR;

namespace Inventory.Api.Hubs;

public class StockHub : Hub
{
    public Task JoinStockWatchers()
        => Groups.AddToGroupAsync(Context.ConnectionId, SignalRStockNotifier.StockWatchersGroup);

    public Task LeaveStockWatchers()
        => Groups.RemoveFromGroupAsync(Context.ConnectionId, SignalRStockNotifier.StockWatchersGroup);
}
