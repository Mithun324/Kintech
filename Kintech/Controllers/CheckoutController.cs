using Kintech.Data;
using Kintech.Models;
using Kintech.Services.Interfaces;
using Kintech.ViewModels.Checkout;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kintech.Controllers;

public class CheckoutController : Controller
{
    private readonly IOrderService _orderService;
    private readonly ICartService _cartService;
    private readonly AppDbContext _context;
    private readonly ILogger<CheckoutController> _logger;

    public CheckoutController(
        IOrderService orderService,
        ICartService cartService,
        AppDbContext context,
        ILogger<CheckoutController> logger)
    {
        _orderService = orderService;
        _cartService = cartService;
        _context = context;
        _logger = logger;
    }

    // =========================
    // GET: /Checkout
    // =========================
    public async Task<IActionResult> Index()
    {
        var cart = _cartService.GetCart();

        if (!cart.Any())
            return RedirectToAction("Index", "Cart");

        // ✅ LOAD ACTIVE DELIVERY SLOTS
        ViewBag.DeliverySlots = await _context.DeliverySlots
            .Where(x => x.IsActive)
            .OrderBy(x => x.Id)
            .ToListAsync();

        return View(cart);
    }

    // =========================
    // POST: /Checkout/PlaceOrder
    // =========================
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PlaceOrder(PlaceOrderViewModel vm)
    {
        var cart = _cartService.GetCart();

        // ✅ RELOAD DELIVERY SLOTS IF VALIDATION FAILS
        ViewBag.DeliverySlots = await _context.DeliverySlots
            .Where(x => x.IsActive)
            .OrderBy(x => x.Id)
            .ToListAsync();

        if (!ModelState.IsValid)
        {
            return View("Index", cart);
        }

        if (!cart.Any())
        {
            return RedirectToAction("Index", "Cart");
        }

        // ✅ VERIFY SLOT EXISTS
        var slot = await _context.DeliverySlots
            .FirstOrDefaultAsync(x =>
                x.Id == vm.DeliverySlotId &&
                x.IsActive);

        if (slot == null)
        {
            ModelState.AddModelError("", "Invalid delivery slot.");

            return View("Index", cart);
        }

        // ✅ CREATE ORDER
        var order = new Order
        {
            CustomerName = vm.Name,
            Address = vm.Address,
            CustomerEmail = vm.Email,
            PhoneNumber = vm.Phone,

            // ORDER DATE/TIME
            OrderDate = DateTime.Now,

            // DELIVERY
            DeliveryDate = vm.DeliveryDate,
            DeliverySlotId = vm.DeliverySlotId,

            // TOTAL
            TotalAmount = cart.Sum(x => x.Price * x.Quantity),

            // ITEMS
            OrderItems = cart.Select(x => new OrderItem
            {
                ProductId = x.ProductId,
                ProductName = x.Name,
                Price = x.Price,
                Quantity = x.Quantity
            }).ToList()
        };

        await _orderService.CreateOrderAsync(order);

        _logger.LogInformation(
            "Order placed by {Email}, Total: {Total}",
            vm.Email,
            order.TotalAmount
        );

        // ✅ CLEAR CART
        _cartService.ClearCart();

        TempData["Success"] =
            "Your order has been placed successfully!";

        return RedirectToAction("Success");
    }

    // =========================
    // GET: /Checkout/Success
    // =========================
    public IActionResult Success()
    {
        return View();
    }
}