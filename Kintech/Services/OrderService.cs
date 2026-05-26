using Kintech.Data;
using Kintech.Models;
using Kintech.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Kintech.Services;

public class OrderService : IOrderService
{
    private readonly AppDbContext _context;
    private readonly ILogger<OrderService> _logger;

    public OrderService(
        AppDbContext context,
        ILogger<OrderService> logger)
    {
        _context = context;
        _logger = logger;
    }

    // =========================
    // CREATE ORDER
    // =========================
    public async Task CreateOrderAsync(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);

        _context.Orders.Add(order);

        await _context.SaveChangesAsync();

        _logger.LogInformation(
            "Order created: ID {Id} for {Email}",
            order.Id,
            order.CustomerEmail
        );
    }

    // =========================
    // GET ALL ORDERS
    // =========================
    public async Task<List<Order>> GetAllOrdersAsync()
    {
        return await _context.Orders
            .AsNoTracking()

            // ✅ LOAD ITEMS
            .Include(o => o.OrderItems)

            // ✅ LOAD DELIVERY SLOT
            .Include(o => o.DeliverySlot)

            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
    }

    // =========================
    // GET ORDER BY ID
    // =========================
    public async Task<Order?> GetByIdAsync(int orderId)
    {
        return await _context.Orders
            .AsNoTracking()

            // ✅ LOAD ITEMS
            .Include(o => o.OrderItems)

            // ✅ LOAD DELIVERY SLOT
            .Include(o => o.DeliverySlot)

            .FirstOrDefaultAsync(o => o.Id == orderId);
    }

    // =========================
    // ORDER COUNT
    // =========================
    public async Task<int> GetCountAsync()
    {
        return await _context.Orders.CountAsync();
    }

    // =========================
    // TOTAL REVENUE
    // =========================
    public async Task<decimal> GetTotalRevenueAsync()
    {
        return await _context.Orders
            .Where(o => o.Status != OrderStatus.Cancelled)
            .SumAsync(o => o.TotalAmount);
    }

    // =========================
    // RECENT ORDERS
    // =========================
    public async Task<List<Order>> GetRecentAsync(int limit)
    {
        return await _context.Orders
            .AsNoTracking()

            // ✅ LOAD ITEMS
            .Include(o => o.OrderItems)

            // ✅ LOAD DELIVERY SLOT
            .Include(o => o.DeliverySlot)

            .OrderByDescending(o => o.OrderDate)
            .Take(limit)
            .ToListAsync();
    }

    // =========================
    // UPDATE STATUS
    // =========================
    public async Task UpdateStatusAsync(
        int orderId,
        OrderStatus status)
    {
        var order = await _context.Orders
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order == null)
        {
            _logger.LogWarning(
                "Status update failed — Order {Id} not found.",
                orderId
            );

            throw new KeyNotFoundException(
                $"Order {orderId} not found."
            );
        }

        order.Status = status;

        await _context.SaveChangesAsync();

        _logger.LogInformation(
            "Order {Id} status updated to {Status}.",
            orderId,
            status
        );
    }
}
