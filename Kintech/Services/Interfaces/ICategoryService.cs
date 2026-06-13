using Kintech.Models;

namespace Kintech.Services.Interfaces;

public interface ICategoryService
{
    Task<List<Category>> GetAllAsync();
    Task<List<Category>> GetSubCategoriesAsync();  
    Task<List<Category>> GetRootCategoriesAsync();
}