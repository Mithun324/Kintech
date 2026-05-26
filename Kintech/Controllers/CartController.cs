using Kintech.Models;
using Kintech.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Kintech.Controllers;

public class CartController : Controller
{
    private readonly IProductService _productService;   // ✅ interface
    private readonly ICartService _cartService;         // ✅ shared cart logic

    public CartController(IProductService productService, ICartService cartService)
    {
        _productService = productService;
        _cartService = cartService;
    }

    // GET: /Cart
    public IActionResult Index()
    {
        var cart = _cartService.GetCart();
        return View(cart);
    }

    // POST: /Cart/Add
    [HttpPost]
    [ValidateAntiForgeryToken]                          // ✅ CSRF protection
    public async Task<IActionResult> Add(int id)
    {
        var product = await _productService.GetByIdAsync(id);

        if (product is null)                            // ✅ null check
        {
            TempData["Error"] = "Product not found.";
            return RedirectToAction("Index");
        }

        var cart = _cartService.GetCart();
        var item = cart.FirstOrDefault(x => x.ProductId == id);

        if (item is null)
        {
            // ✅ check stock before adding
            if (product.Stock <= 0)
            {
                TempData["Error"] = $"'{product.Name}' is out of stock.";
                return RedirectToAction("Index", "Product");
            }

            cart.Add(new CartItem
            {
                ProductId = product.Id,
                Name = product.Name,
                Price = product.Price,
                Quantity = 1,
                ImageUrl = product.ImageUrl
            });
        }
        else
        {
            // ✅ check stock before increasing
            if (item.Quantity >= product.Stock)
            {
                TempData["Error"] = $"Only {product.Stock} units available.";
                return RedirectToAction("Index");
            }

            item.Quantity++;
        }

        _cartService.SaveCart(cart);
        TempData["Success"] = $"'{product.Name}' added to cart.";
        return RedirectToAction("Index");
    }

    // POST: /Cart/Remove
    [HttpPost]
    [ValidateAntiForgeryToken]                          // ✅ CSRF protection
    public IActionResult Remove(int id)
    {
        var cart = _cartService.GetCart();
        var item = cart.FirstOrDefault(x => x.ProductId == id);

        if (item is not null)
            cart.Remove(item);

        _cartService.SaveCart(cart);
        return RedirectToAction("Index");
    }

    // POST: /Cart/Increase
    [HttpPost]
    [ValidateAntiForgeryToken]                          // ✅ CSRF protection
    public async Task<IActionResult> Increase(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        var cart = _cartService.GetCart();
        var item = cart.FirstOrDefault(x => x.ProductId == id);

        if (item is not null)
        {
            // ✅ stock check on increase
            if (product is not null && item.Quantity >= product.Stock)
            {
                TempData["Error"] = $"Only {product.Stock} units available.";
                return RedirectToAction("Index");
            }

            item.Quantity++;
        }

        _cartService.SaveCart(cart);
        return RedirectToAction("Index");
    }

    // POST: /Cart/Decrease
    [HttpPost]
    [ValidateAntiForgeryToken]                          // ✅ CSRF protection
    public IActionResult Decrease(int id)
    {
        var cart = _cartService.GetCart();
        var item = cart.FirstOrDefault(x => x.ProductId == id);

        if (item is not null)
        {
            item.Quantity--;
            if (item.Quantity <= 0)
                cart.Remove(item);
        }

        _cartService.SaveCart(cart);
        return RedirectToAction("Index");
    }

    // GET: /Cart/Count — usable via AJAX
    [HttpGet]
    public IActionResult Count()
    {
        return Json(_cartService.GetCartCount());       // ✅ proper action
    }
}