namespace Kintech.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;   // ✅
    public int? ParentId { get; set; }
    public Category? Parent { get; set; }              // ✅ nullable nav prop
    public List<Category> SubCategories { get; set; } = [];  // ✅
    public List<Product> Products { get; set; } = [];        // ✅
}