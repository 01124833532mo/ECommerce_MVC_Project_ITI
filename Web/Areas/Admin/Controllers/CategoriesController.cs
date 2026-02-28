using EcommerceIti.Application.Interfaces;
using EcommerceIti.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace EcommerceIti.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class CategoriesController : Controller
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    private async Task LoadCategoriesAsync(int? excludeId = null)
    {
        var categories = await _categoryService.GetAllAsync();
        if (excludeId.HasValue)
        {
            categories = categories.Where(c => c.CategoryId != excludeId.Value).ToList();
        }

        ViewBag.Categories = categories;
    }

    public async Task<IActionResult> Index()
    {
        var categories = await _categoryService.GetAllAsync();
        return View(categories);
    }

    public async Task<IActionResult> Create()
    {
        await LoadCategoriesAsync();
        return View(new CategoryVm());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CategoryVm vm)
    {
        if (!ModelState.IsValid)
        {
            await LoadCategoriesAsync();
            return View(vm);
        }
        await _categoryService.CreateAsync(vm);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var vm = await _categoryService.GetByIdAsync(id);
        if (vm == null)
        {
            return NotFound();
        }
        await LoadCategoriesAsync(id);
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(CategoryVm vm)
    {
        if (!ModelState.IsValid)
        {
            await LoadCategoriesAsync(vm.CategoryId);
            return View(vm);
        }
        await _categoryService.UpdateAsync(vm);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _categoryService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
