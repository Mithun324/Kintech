using Kintech.Models;
using Kintech.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kintech.Controllers;

[Authorize(Roles = "Admin")]
public class OrderController : Controller
{
    private readonly IOrderService _orderService;          // ✅ interface
    private readonly ILogger<OrderController> _logger;

    public OrderController(IOrderService orderService, ILogger<OrderController> logger)
    {
        _orderService = orderService;
        _logger = logger;
    }

    // GET: /Order
    public async Task<IActionResult> Index()
    {
        var orders = await _orderService.GetAllOrdersAsync();
        return View(orders);
    }

    // POST: /Order/UpdateStatus
    [HttpPost]
    [ValidateAntiForgeryToken]                             // ✅ CSRF protection
    public async Task<IActionResult> UpdateStatus(int id, OrderStatus status)  // ✅ enum
    {
        if (!Enum.IsDefined(typeof(OrderStatus), status)) // ✅ validate enum value
        {
            TempData["Error"] = "Invalid status value.";
            return RedirectToAction(nameof(Index));
        }

        var order = await _orderService.GetByIdAsync(id);
        if (order is null)                                 // ✅ guard against bad ID
        {
            TempData["Error"] = $"Order {id} not found.";
            return RedirectToAction(nameof(Index));
        }

        await _orderService.UpdateStatusAsync(id, status);

        _logger.LogInformation("Order {Id} status updated to {Status} by {Admin}.",
            id, status, User.Identity?.Name);

        TempData["Success"] = $"Order {id} updated to {status}.";
        return RedirectToAction(nameof(Index));
    }
}