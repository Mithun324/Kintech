using Kintech.Data;
using Kintech.Models;
using Kintech.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Kintech.Services;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _context;
    private readonly ILogger<CategoryService> _logger;

    public CategoryService(AppDbContext context, ILogger<CategoryService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<Category>> GetAllAsync()
    {
        return await _context.Categories
            .AsNoTracking()
            .Include(c => c.SubCategories)
            .ToListAsync();
    }

    public async Task<List<Category>> GetSubCategoriesAsync()
    {
        return await _context.Categories
            .AsNoTracking()
            .Where(c => c.ParentId != null)
            .ToListAsync();
    }

    public async Task<List<Category>> GetRootCategoriesAsync()
    {
        return await _context.Categories
            .AsNoTracking()
            .Where(c => c.ParentId == null)
            .Include(c => c.SubCategories) 
            .OrderBy(c => c.Name)
            .ToListAsync();
    }
}