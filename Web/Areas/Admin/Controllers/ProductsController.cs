using EcommerceIti.Application.Interfaces;
using EcommerceIti.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceIti.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class ProductsController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;

    public ProductsController(IProductService productService, ICategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
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
    public async Task<IActionResult> Create(ProductEditVm vm)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = await _categoryService.GetAllAsync();
            return View(vm);
        }

        await _productService.CreateAsync(vm);
        return RedirectToAction(nameof(Index));
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
    public async Task<IActionResult> Edit(ProductEditVm vm)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = await _categoryService.GetAllAsync();
            return View(vm);
        }

        await _productService.UpdateAsync(vm);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _productService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
