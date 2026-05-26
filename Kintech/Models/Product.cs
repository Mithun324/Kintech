using System.ComponentModel.DataAnnotations;

namespace Kintech.Models;

public class Product
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public decimal Price { get; set; }

    [Required]
    public int Stock { get; set; }

    public string? Description { get; set; }
    public string? ImageUrl { get; set; }

    [Required(ErrorMessage = "Please select a category")]
    public int? CategoryId { get; set; }

    public Category? Category { get; set; }
}