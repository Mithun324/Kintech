using Kintech.Models;

namespace Kintech.Services.Interfaces;

public interface IProductService
{
    Task<List<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int id);
    Task<int> GetCountAsync();     
    Task CreateAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(int id);

    Task<List<Product>> GetByCategoryAsync(int categoryId); 
}