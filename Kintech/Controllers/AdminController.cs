using Kintech.Services;
using Kintech.ViewModels.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Kintech.Services.Interfaces;

namespace Kintech.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IProductService _productService;   // ✅ interface
    private readonly IOrderService _orderService;       // ✅ interface
    private readonly ILogger<AdminController> _logger;

    public AdminController(
        IProductService productService,
        IOrderService orderService,
        ILogger<AdminController> logger)
    {
        _productService = productService;
        _orderService = orderService;
        _logger = logger;
    }

    // GET: /Admin
    public async Task<IActionResult> Index()
    {
        // ✅ Use lightweight summary calls, not full collection loads
        var dashboard = new DashboardViewModel
        {
            ProductCount = await _productService.GetCountAsync(),
            OrderCount = await _orderService.GetCountAsync(),
            TotalRevenue = await _orderService.GetTotalRevenueAsync(),
            RecentOrders = await _orderService.GetRecentAsync(limit: 10)
        };

        return View(dashboard);  // ✅ strongly-typed ViewModel
    }

    // GET: /Admin/Orders
    public async Task<IActionResult> Orders()
    {
        var orders = await _orderService.GetAllOrdersAsync();
        return View(orders);
    }

    // GET: /Admin/Products
    public async Task<IActionResult> Products()  // ✅ no redundant [Authorize]
    {
        var products = await _productService.GetAllAsync();
        return View(products);
    }

    // POST: /Admin/UpdateStatus
    [HttpPost]
    [ValidateAntiForgeryToken]                   // ✅ CSRF protection
    public async Task<IActionResult> UpdateStatus(UpdateStatusViewModel model)
    {
        if (!ModelState.IsValid)                 // ✅ validate before acting
        {
            TempData["Error"] = "Invalid status update request.";
            return RedirectToAction(nameof(Orders));
        }

        var order = await _orderService.GetByIdAsync(model.OrderId);
        if (order is null)                       // ✅ guard against bad IDs
        {
            _logger.LogWarning("Admin attempted to update non-existent order {Id}.", model.OrderId);
            TempData["Error"] = $"Order {model.OrderId} not found.";
            return RedirectToAction(nameof(Orders));
        }

        await _orderService.UpdateStatusAsync(model.OrderId, model.Status);

        _logger.LogInformation(                  // ✅ audit trail
            "Admin {Admin} updated Order {Id} to {Status}.",
            User.Identity?.Name, model.OrderId, model.Status);

        TempData["Success"] = $"Order {model.OrderId} updated to {model.Status}.";
        return RedirectToAction(nameof(Orders));
    }
}