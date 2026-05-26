using Kintech.Data;
using Kintech.Models;
using Kintech.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Kintech.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _context;
    private readonly ILogger<ProductService> _logger;

    public ProductService(AppDbContext context, ILogger<ProductService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await _context.Products
            .AsNoTracking()                             // ✅ read-only, faster
            .ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products
            .AsNoTracking()                             // ✅ read-only
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<int> GetCountAsync()
    {
        return await _context.Products.CountAsync();   // ✅ no full load
    }

    public async Task CreateAsync(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);    // ✅ guard clause

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Product created: {Name} (ID: {Id})",
            product.Name, product.Id);
    }

    public async Task UpdateAsync(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);    // ✅ guard clause

        var exists = await _context.Products
            .AnyAsync(p => p.Id == product.Id);

        if (!exists)                                   // ✅ explicit not-found check
        {
            _logger.LogWarning("Update failed — Product {Id} not found.", product.Id);
            throw new KeyNotFoundException($"Product {product.Id} not found.");
        }

        _context.Products.Update(product);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Product updated: ID {Id}", product.Id);
    }

    public async Task DeleteAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product is null)                           // ✅ explicit not-found check
        {
            _logger.LogWarning("Delete failed — Product {Id} not found.", id);
            throw new KeyNotFoundException($"Product {id} not found.");
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Product deleted: ID {Id}", id);
    }

    public async Task<List<Product>> GetByCategoryAsync(int categoryId)
    {
        return await _context.Products
            .AsNoTracking()
            .Where(p => p.CategoryId == categoryId)          // ✅ filtered in DB
            .ToListAsync();
    }
}