using Inventory.Api.Contracts;
using Inventory.Api.Data;
using Inventory.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<ProductListDto>>> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50,
        [FromQuery] string? search = null,
        CancellationToken ct = default)
    {
        pageSize = Math.Clamp(pageSize, 1, 200);
        page = Math.Max(page, 1);

        var query = db.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(p => p.Name.Contains(search) || p.Sku.Contains(search));

        var total = await query.CountAsync(ct);

        var products = await query
            .OrderBy(p => p.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        var items = new List<ProductListDto>();
        foreach (var product in products)
        {
            var category = await db.Categories
                .Where(c => c.Id == product.CategoryId)
                .Select(c => c.Name)
                .SingleAsync(ct);

            items.Add(new ProductListDto(
                product.Id,
                product.Name,
                product.Sku,
                category,
                product.UnitPrice,
                product.Stock,
                product.ReorderThreshold));
        }

        return new PagedResult<ProductListDto>(items, total, page, pageSize);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductDetailDto>> Get(int id, CancellationToken ct)
    {
        var product = await db.Products
            .Where(p => p.Id == id)
            .Select(p => new ProductDetailDto(
                p.Id, p.Name, p.Sku, p.CategoryId, p.Category.Name, p.UnitPrice, p.Stock, p.ReorderThreshold))
            .SingleOrDefaultAsync(ct);

        return product is null ? NotFound() : product;
    }

    [HttpPost]
    public async Task<ActionResult<ProductDetailDto>> Create(UpsertProductRequest request, CancellationToken ct)
    {
        if (!await db.Categories.AnyAsync(c => c.Id == request.CategoryId, ct))
            return BadRequest($"Category {request.CategoryId} does not exist.");

        var product = new Product
        {
            Name = request.Name,
            Sku = request.Sku,
            CategoryId = request.CategoryId,
            UnitPrice = request.UnitPrice,
            Stock = request.Stock,
            ReorderThreshold = request.ReorderThreshold
        };

        db.Products.Add(product);
        await db.SaveChangesAsync(ct);

        var created = new ProductDetailDto(
            product.Id,
            product.Name,
            product.Sku,
            product.CategoryId,
            await db.Categories.Where(c => c.Id == product.CategoryId).Select(c => c.Name).SingleAsync(ct),
            product.UnitPrice,
            product.Stock,
            product.ReorderThreshold);

        return CreatedAtAction(nameof(Get), new { id = product.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpsertProductRequest request, CancellationToken ct)
    {
        var product = await db.Products.FindAsync([id], ct);
        if (product is null) return NotFound();

        product.Name = request.Name;
        product.Sku = request.Sku;
        product.CategoryId = request.CategoryId;
        product.UnitPrice = request.UnitPrice;
        product.Stock = request.Stock;
        product.ReorderThreshold = request.ReorderThreshold;

        await db.SaveChangesAsync(ct);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var product = await db.Products.FindAsync([id], ct);
        if (product is null) return NotFound();

        db.Products.Remove(product);
        await db.SaveChangesAsync(ct);
        return NoContent();
    }
}
