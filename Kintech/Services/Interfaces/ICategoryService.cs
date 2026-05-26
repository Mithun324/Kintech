using Kintech.Models;

namespace Kintech.Services.Interfaces;

public interface ICategoryService
{
    Task<List<Category>> GetAllAsync();
    Task<List<Category>> GetSubCategoriesAsync();   // ParentId != null
    Task<List<Category>> GetRootCategoriesAsync();  // ParentId == null
}