using Kintech.Models;

namespace Kintech.ViewModels.Admin;

public class DashboardViewModel
{
    public int ProductCount { get; set; }
    public int OrderCount { get; set; }
    public decimal TotalRevenue { get; set; }
    public IEnumerable<Order> RecentOrders { get; set; } = [];
}