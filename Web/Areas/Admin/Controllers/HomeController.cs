using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EcommerceIti.Application.Interfaces;

namespace EcommerceIti.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class HomeController : Controller
{
    private readonly ICategoryService _categoryService;
    private readonly IProductService _productService;
    private readonly IOrderService _orderService;

    public HomeController(ICategoryService categoryService, IProductService productService, IOrderService orderService)
    {
        _categoryService = categoryService;
        _productService = productService;
        _orderService = orderService;
    }

    public async Task<IActionResult> Index()
    {
        var categories = await _categoryService.GetAllAsync();
        var orders = await _orderService.GetAllOrdersAsync();

        ViewBag.CategoryCount = categories.Count;
        ViewBag.OrderCount = orders.Count;
        ViewBag.RecentOrders = orders.Take(5).ToList();
        return View();
    }
}
