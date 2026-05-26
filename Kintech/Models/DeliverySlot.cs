namespace Kintech.Models;

public class DeliverySlot
{
    public int Id { get; set; }

    // Example:
    // "10 AM - 12 PM"
    public string TimeSlot { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public List<Order>? Orders { get; set; }
}