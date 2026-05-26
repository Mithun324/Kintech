using Kintech.Models;

namespace Kintech.Services.Interfaces;

public interface IOrderService
{
    Task CreateOrderAsync(Order order);
    Task<List<Order>> GetAllOrdersAsync();
    Task<Order?> GetByIdAsync(int orderId);             // ✅ null-safe lookup
    Task<int> GetCountAsync();                          // ✅ for dashboard
    Task<decimal> GetTotalRevenueAsync();               // ✅ for dashboard
    Task<List<Order>> GetRecentAsync(int limit);        // ✅ for dashboard
    Task UpdateStatusAsync(int orderId, OrderStatus status); // ✅ enum not string
}