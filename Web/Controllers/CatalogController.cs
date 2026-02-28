using EcommerceIti.Application.Interfaces;
using EcommerceIti.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceIti.Web.Controllers;

public class CatalogController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;

    public CatalogController(IProductService productService, ICategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
    }

    public async Task<IActionResult> Index(int? categoryId, string? q, string? sort, int page = 1)
    {
        var query = new ProductListQuery(categoryId, q, sort, page, 12);
        var vm = await _productService.GetPagedAsync(query);
        ViewBag.Categories = await _categoryService.GetAllAsync();
        return View(vm);
    }

    public async Task<IActionResult> Details(int id)
    {
        var vm = await _productService.GetDetailsAsync(id);
        if (vm == null)
        {
            return NotFound();
        }
        return View(vm);
    }
}
