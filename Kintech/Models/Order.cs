using System.ComponentModel.DataAnnotations;

namespace Kintech.Models;

public class Order
{
    public int Id { get; set; }

    public string? CustomerName { get; set; }

    public string? Address { get; set; }

    [Required]
    [EmailAddress]
    public string CustomerEmail { get; set; } = string.Empty;

    [Required]
    [Phone]
    public string PhoneNumber { get; set; } = string.Empty;

    public decimal TotalAmount { get; set; }

    public DateTime OrderDate { get; set; }

   
    public DateTime? DeliveryDate { get; set; }

   
    public int? DeliverySlotId { get; set; }

    public DeliverySlot? DeliverySlot { get; set; }

    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    public List<OrderItem> OrderItems { get; set; } = [];
}