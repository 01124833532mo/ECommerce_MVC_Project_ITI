using EcommerceIti.Application.Interfaces;
using EcommerceIti.Application.Models;
using EcommerceIti.Application.ViewModels;
using EcommerceIti.Application.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceIti.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class ProductsController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;
    private readonly IWebHostEnvironment _environment;

    public ProductsController(IProductService productService, ICategoryService categoryService, IWebHostEnvironment environment)
    {
        _productService = productService;
        _categoryService = categoryService;
        _environment = environment;
    }

    public async Task<IActionResult> Index(int? categoryId)
    {
        var vm = await _productService.GetPagedAsync(new ProductListQuery(categoryId, null, null, 1, 100, IncludeInactive: true));
        ViewBag.Categories = await _categoryService.GetAllAsync();
        return View(vm);
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Categories = await _categoryService.GetAllAsync();
        return View(new ProductEditVm { IsActive = true });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductEditVm vm, IFormFile? imageFile)
    {
        vm.SKU = vm.SKU.Trim();

        if (await _productService.IsSkuInUseAsync(vm.SKU))
        {
            ModelState.AddModelError(nameof(vm.SKU), "This SKU already exists. Please enter a different SKU.");
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Categories = await _categoryService.GetAllAsync();
            return View(vm);
        }

        var uploadedImagePath = await SaveImageAsync(imageFile);
        if (!string.IsNullOrWhiteSpace(uploadedImagePath))
        {
            vm.ImageUrl = uploadedImagePath;
        }

        try
        {
            await _productService.CreateAsync(vm);
            return RedirectToAction(nameof(Index));
        }
        catch (DuplicateSkuException)
        {
            ModelState.AddModelError(nameof(vm.SKU), "This SKU already exists. Please enter a different SKU.");
            ViewBag.Categories = await _categoryService.GetAllAsync();
            return View(vm);
        }
    }

    public async Task<IActionResult> Edit(int id)
    {
        var vm = await _productService.GetForEditAsync(id);
        if (vm == null)
        {
            return NotFound();
        }
        ViewBag.Categories = await _categoryService.GetAllAsync();
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ProductEditVm vm, IFormFile? imageFile)
    {
        vm.SKU = vm.SKU.Trim();

        if (await _productService.IsSkuInUseAsync(vm.SKU, vm.ProductId))
        {
            ModelState.AddModelError(nameof(vm.SKU), "This SKU already exists. Please enter a different SKU.");
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Categories = await _categoryService.GetAllAsync();
            return View(vm);
        }

        var uploadedImagePath = await SaveImageAsync(imageFile);
        if (!string.IsNullOrWhiteSpace(uploadedImagePath))
        {
            vm.ImageUrl = uploadedImagePath;
        }

        try
        {
            await _productService.UpdateAsync(vm);
            return RedirectToAction(nameof(Index));
        }
        catch (DuplicateSkuException)
        {
            ModelState.AddModelError(nameof(vm.SKU), "This SKU already exists. Please enter a different SKU.");
            ViewBag.Categories = await _categoryService.GetAllAsync();
            return View(vm);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _productService.DeleteAsync(id);

        TempData["StatusMessage"] = result switch
        {
            ProductDeleteResult.Deleted => "Product deleted successfully.",
            ProductDeleteResult.Deactivated => "This product exists in previous orders, so it was marked inactive instead of being deleted.",
            _ => "Product was not found."
        };

        TempData["StatusType"] = result == ProductDeleteResult.Deactivated ? "warning" : "success";
        return RedirectToAction(nameof(Index));
    }

    private async Task<string?> SaveImageAsync(IFormFile? imageFile)
    {
        if (imageFile == null || imageFile.Length == 0)
        {
            return null;
        }

        var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "products");
        Directory.CreateDirectory(uploadsFolder);

        var extension = Path.GetExtension(imageFile.FileName);
        var fileName = $"{Guid.NewGuid():N}{extension}";
        var filePath = Path.Combine(uploadsFolder, fileName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await imageFile.CopyToAsync(stream);

        return $"/uploads/products/{fileName}";
    }
}
