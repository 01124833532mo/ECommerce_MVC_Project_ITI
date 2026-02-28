using EcommerceIti.Application.Interfaces;
using EcommerceIti.Application.ViewModels;
using EcommerceIti.Web.Services;
using EcommerceIti.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceIti.Web.Controllers;

[Authorize]
public class OrdersController : Controller
{
    private readonly IOrderService _orderService;
    private readonly IAddressService _addressService;
    private readonly CartSessionService _cartSession;

    public OrdersController(IOrderService orderService, IAddressService addressService, CartSessionService cartSession)
    {
        _orderService = orderService;
        _addressService = addressService;
        _cartSession = cartSession;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.GetUserId();
        var orders = await _orderService.GetOrdersAsync(userId);
        return View(orders);
    }

    public async Task<IActionResult> Details(int id)
    {
        var userId = User.GetUserId();
        var order = await _orderService.GetOrderAsync(id, userId);
        if (order == null)
        {
            return NotFound();
        }
        return View(order);
    }

    [HttpGet]
    public async Task<IActionResult> Checkout()
    {
        var userId = User.GetUserId();
        var addresses = await _addressService.GetForUserAsync(userId);
        var cart = _cartSession.GetCart();
        var vm = new CheckoutVm
        {
            Addresses = addresses,
            SelectedAddressId = addresses.FirstOrDefault(a => a.IsDefault)?.AddressId,
            Cart = cart
        };
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Checkout(CheckoutVm vm)
    {
        var userId = User.GetUserId();
        var cart = _cartSession.GetCart();
        if (cart.Items.Count == 0)
        {
            ModelState.AddModelError(string.Empty, "Cart is empty");
        }

        if (!ModelState.IsValid)
        {
            vm.Addresses = await _addressService.GetForUserAsync(userId);
            vm.Cart = cart;
            return View(vm);
        }

        var orderId = await _orderService.CheckoutAsync(userId, vm.SelectedAddressId!.Value, cart);
        _cartSession.Clear();
        return RedirectToAction(nameof(Details), new { id = orderId });
    }
}
