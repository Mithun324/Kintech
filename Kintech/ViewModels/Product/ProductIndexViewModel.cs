
using Kintech.Models;

namespace Kintech.ViewModels.Product;

public class ProductIndexViewModel
{
    public List<Kintech.Models.Product> Products { get; set; } = [];
    public List<Kintech.Models.Category> Categories { get; set; } = [];
    public int? SelectedCategoryId { get; set; }
    public string? SearchTerm { get; set; }
}