using System.ComponentModel.DataAnnotations;

namespace Kintech.ViewModels.Checkout;

public class PlaceOrderViewModel
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(300)]
    public string Address { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Phone]
    public string Phone { get; set; } = string.Empty;

    [Required]
    public DateTime DeliveryDate { get; set; }

    [Required]
    public int DeliverySlotId { get; set; }
}