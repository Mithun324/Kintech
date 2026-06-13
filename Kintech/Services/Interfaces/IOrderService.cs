using Kintech.Models;

namespace Kintech.Services.Interfaces;

public interface IOrderService
{
    Task CreateOrderAsync(Order order);
    Task<List<Order>> GetAllOrdersAsync();
    Task<Order?> GetByIdAsync(int orderId);
    Task<int> GetCountAsync();
    Task<decimal> GetTotalRevenueAsync();
    Task<List<Order>> GetRecentAsync(int limit);
    Task UpdateStatusAsync(int orderId, OrderStatus status);
}