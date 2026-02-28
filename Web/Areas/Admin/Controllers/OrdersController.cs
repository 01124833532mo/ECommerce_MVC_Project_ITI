using EcommerceIti.Application.Interfaces;
using EcommerceIti.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceIti.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class OrdersController : Controller
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public async Task<IActionResult> Index()
    {
        var orders = await _orderService.GetAllOrdersAsync();
        return View(orders);
    }

    public async Task<IActionResult> Details(int id)
    {
        var order = await _orderService.GetOrderAsync(id, null, asAdmin: true);
        if (order == null)
        {
            return NotFound();
        }
        return View(order);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(int id, OrderStatus status)
    {
        await _orderService.UpdateStatusAsync(id, status);
        return RedirectToAction(nameof(Details), new { id });
    }
}
