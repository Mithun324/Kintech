using Kintech.Models;

namespace Kintech.ViewModels.Product;

public class ProductIndexViewModel
{
    public List<Kintech.Models.Product> Products { get; set; } = [];
    public List<Category> Categories { get; set; } = [];
    public int? SelectedCategoryId { get; set; }
}