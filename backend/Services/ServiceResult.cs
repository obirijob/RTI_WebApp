namespace Inventory.Api.Services;

public abstract record ServiceResult
{
    public record Ok : ServiceResult;
    public record Ok<T>(T Value) : ServiceResult;
    public record Invalid(string Error) : ServiceResult;
    public record NotFound(string Error) : ServiceResult;
    public record Conflict(string Error) : ServiceResult;
}
