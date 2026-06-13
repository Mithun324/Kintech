namespace Kintech.Models;

public class DeliverySlot
{
    public int Id { get; set; }

    public string TimeSlot { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public List<Order>? Orders { get; set; }
}