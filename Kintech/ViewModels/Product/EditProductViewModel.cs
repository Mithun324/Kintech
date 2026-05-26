using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Kintech.ViewModels.Product;

public class EditProductViewModel
{
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
    public decimal Price { get; set; }

    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative.")]
    public int Stock { get; set; }

    [StringLength(2000)]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Please select a category.")]
    public int? CategoryId { get; set; }

    public string? ExistingImageUrl { get; set; }   // ✅ keep existing image
    public IFormFile? ImageFile { get; set; }

    public List<SelectListItem> Categories { get; set; } = [];
}