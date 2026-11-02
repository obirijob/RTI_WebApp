using System.Text.Json.Serialization;
using Inventory.Api.Data;
using Inventory.Api.Hubs;
using Inventory.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddOpenApi();
builder.Services.AddSignalR();
builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy
    .WithOrigins("http://localhost:4200")
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()));

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<OrderService>();
builder.Services.AddSingleton<IStockNotifier, SignalRStockNotifier>();

var app = builder.Build();

if (app.Environment.IsDevelopment()) app.MapOpenApi();

app.UseCors();
app.MapControllers();
app.MapHub<StockHub>("/hubs/stock");
app.MapGet("/health", () => Results.Ok(new { status = "healthy" }));

await using (var scope = app.Services.CreateAsyncScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
    await DatabaseSeeder.SeedAsync(db);
}

app.Run();

public partial class Program;
