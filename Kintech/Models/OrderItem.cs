using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace Kintech.Models;

public class OrderItem
{
    public int Id { get; set; }

    [Required]
    public int ProductId { get; set; }

    public string? ProductName { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }

    public int OrderId { get; set; }

    [JsonIgnore]   
    public Order Order { get; set; } = null!;
}