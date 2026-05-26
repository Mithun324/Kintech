using Kintech.Models;
using Kintech.Services.Interfaces;
using Kintech.ViewModels.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace Kintech.Controllers;

public class ProductController : Controller
{
    private readonly IProductService _productService;       // ✅ interface
    private readonly ICategoryService _categoryService;     // ✅ service not DbContext
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<ProductController> _logger;

    private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png", ".webp"];
    private const long MaxImageSizeBytes = 5 * 1024 * 1024; // 5MB

    public ProductController(
        IProductService productService,
        ICategoryService categoryService,
        IWebHostEnvironment env,
        ILogger<ProductController> logger)
    {
        _productService = productService;
        _categoryService = categoryService;
        _env = env;
        _logger = logger;
    }

    // ===================== INDEX =====================
    public async Task<IActionResult> Index(int? categoryId)
    {
        var vm = new ProductIndexViewModel
        {
            Products = categoryId.HasValue
                ? await _productService.GetByCategoryAsync(categoryId.Value)
                : await _productService.GetAllAsync(),
            Categories = await _categoryService.GetRootCategoriesAsync(),
            SelectedCategoryId = categoryId
        };

        return View(vm);
    }

    // ===================== CREATE =====================
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create()
    {
        var vm = new CreateProductViewModel
        {
            Categories = await GetCategorySelectListAsync()
        };

        return View(vm);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]                              // ✅ CSRF protection
    public async Task<IActionResult> Create(CreateProductViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            vm.Categories = await GetCategorySelectListAsync();
            return View(vm);
        }

        string? imageUrl = null;
        if (vm.ImageFile is not null)
        {
            var (success, result) = await TrySaveImageAsync(vm.ImageFile);
            if (!success)
            {
                ModelState.AddModelError("ImageFile", result);
                vm.Categories = await GetCategorySelectListAsync();
                return View(vm);
            }
            imageUrl = result;
        }

        var product = new Product
        {
            Name = vm.Name,
            Price = vm.Price,
            Stock = vm.Stock,
            Description = vm.Description,
            CategoryId = vm.CategoryId,
            ImageUrl = imageUrl
        };

        await _productService.CreateAsync(product);

        _logger.LogInformation("Product created: {Name} by {Admin}",
            vm.Name, User.Identity?.Name);

        TempData["Success"] = $"'{vm.Name}' added successfully.";
        return RedirectToAction("Products", "Admin");
    }

    // ===================== EDIT =====================
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product is null) return NotFound();

        var vm = new EditProductViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Stock = product.Stock,
            Description = product.Description,
            CategoryId = product.CategoryId,
            ExistingImageUrl = product.ImageUrl,
            Categories = await GetCategorySelectListAsync(product.CategoryId)
        };

        return View(vm);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]                              // ✅ CSRF protection
    public async Task<IActionResult> Edit(EditProductViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            vm.Categories = await GetCategorySelectListAsync(vm.CategoryId);
            return View(vm);
        }

        var product = await _productService.GetByIdAsync(vm.Id);
        if (product is null) return NotFound();

        if (vm.ImageFile is not null)
        {
            var (success, result) = await TrySaveImageAsync(vm.ImageFile);
            if (!success)
            {
                ModelState.AddModelError("ImageFile", result);
                vm.Categories = await GetCategorySelectListAsync(vm.CategoryId);
                return View(vm);
            }

            DeleteImageFile(product.ImageUrl);              // ✅ remove old image
            product.ImageUrl = result;
        }

        product.Name = vm.Name;
        product.Price = vm.Price;
        product.Stock = vm.Stock;
        product.Description = vm.Description;
        product.CategoryId = vm.CategoryId;

        await _productService.UpdateAsync(product);

        _logger.LogInformation("Product updated: ID {Id} by {Admin}",
            vm.Id, User.Identity?.Name);

        TempData["Success"] = $"'{vm.Name}' updated successfully.";
        return RedirectToAction("Products", "Admin");
    }

    // ===================== DELETE =====================
    [HttpPost]                                              // ✅ must be POST
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product is null) return NotFound();

        DeleteImageFile(product.ImageUrl);                  // ✅ clean up file

        await _productService.DeleteAsync(id);

        _logger.LogInformation("Product deleted: ID {Id} by {Admin}",
            id, User.Identity?.Name);

        TempData["Success"] = "Product deleted successfully.";
        return RedirectToAction("Products", "Admin");
    }

    // ===================== HELPERS =====================
    private async Task<List<SelectListItem>> GetCategorySelectListAsync(int? selectedId = null)
    {
        var categories = await _categoryService.GetSubCategoriesAsync();
        return categories.Select(c => new SelectListItem
        {
            Value = c.Id.ToString(),
            Text = c.Name,
            Selected = c.Id == selectedId
        }).ToList();
    }

    private async Task<(bool success, string result)> TrySaveImageAsync(IFormFile file)
    {
        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!AllowedExtensions.Contains(ext))               // ✅ extension check
            return (false, "Only .jpg, .jpeg, .png, .webp files are allowed.");

        if (file.Length > MaxImageSizeBytes)                // ✅ size check
            return (false, "Image must be under 5MB.");

        string folder = Path.Combine(_env.WebRootPath, "images");
        Directory.CreateDirectory(folder);                  // no-op if exists

        string fileName = Guid.NewGuid() + ext;
        string path = Path.Combine(folder, fileName);

        await using var stream = new FileStream(path, FileMode.Create);
        await file.CopyToAsync(stream);

        return (true, "/images/" + fileName);
    }

    private void DeleteImageFile(string? imageUrl)
    {
        if (string.IsNullOrEmpty(imageUrl)) return;

        var path = Path.Combine(_env.WebRootPath, imageUrl.TrimStart('/'));
        if (System.IO.File.Exists(path))
            System.IO.File.Delete(path);
    }
}