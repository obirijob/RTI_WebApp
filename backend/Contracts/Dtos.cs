namespace Inventory.Api.Contracts;

public record ProductListDto(
    int Id,
    string Name,
    string Sku,
    string Category,
    decimal UnitPrice,
    int Stock,
    int ReorderThreshold);

public record ProductDetailDto(
    int Id,
    string Name,
    string Sku,
    int CategoryId,
    string Category,
    decimal UnitPrice,
    int Stock,
    int ReorderThreshold);

public record PagedResult<T>(IReadOnlyList<T> Items, int Total, int Page, int PageSize);

public record UpsertProductRequest(
    string Name,
    string Sku,
    int CategoryId,
    decimal UnitPrice,
    int Stock,
    int ReorderThreshold);

public record PlaceOrderLine(int ProductId, int Quantity);

public record PlaceOrderRequest(IReadOnlyList<PlaceOrderLine> Lines, string? ConnectionId = null);

public record OrderDto(
    int Id,
    DateTime OrderDate,
    string Status,
    decimal Total,
    IReadOnlyList<OrderLineDto> Lines);

public record OrderLineDto(int ProductId, string ProductName, int Quantity, decimal UnitPrice);

public record RestockRequest(int Quantity);

public record LowStockAlertDto(int ProductId, string ProductName, int Stock, int ReorderThreshold);
