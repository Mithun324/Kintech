using Kintech.Services.Interfaces;
using Kintech.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;

namespace Kintech.Controllers;

public class HomeController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;
    private readonly ILogger<HomeController> _logger;

    public HomeController(
        IProductService productService,
        ICategoryService categoryService,
        ILogger<HomeController> logger)
    {
        _productService = productService;
        _categoryService = categoryService;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var allProducts = await _productService.GetAllAsync();
        var allCategories = await _categoryService.GetSubCategoriesAsync();

        var vm = new HomeViewModel
        {
            FeaturedProducts = allProducts
                .OrderByDescending(p => p.Price)
                .Take(8)
                .ToList(),

            NewArrivals = allProducts
                .OrderByDescending(p => p.Id)
                .Take(8)
                .ToList(),

            Categories = allCategories
        };

        return View(vm);
    }
    /* NewsLetter */
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Subscribe(string email)
    {
        // TODO: wire up to email service
        TempData["Success"] = "Thanks for subscribing!";
        return RedirectToAction(nameof(Index));
    }

}
