using EcommerceIti.Application.Interfaces;
using EcommerceIti.Application.ViewModels;
using EcommerceIti.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceIti.Web.Controllers;

public class CartController : Controller
{
    private readonly IProductService _productService;
    private readonly CartSessionService _cartSession;

    public CartController(IProductService productService, CartSessionService cartSession)
    {
        _productService = productService;
        _cartSession = cartSession;
    }

    public IActionResult Index()
    {
        var cart = _cartSession.GetCart();
        return View(cart);
    }

    [HttpPost]
    public async Task<IActionResult> Add(int productId, int quantity = 1)
    {
        var product = await _productService.GetDetailsAsync(productId);
        if (product == null || !product.IsActive)
        {
            return NotFound();
        }

        var cart = _cartSession.GetCart();
        var existing = cart.Items.FirstOrDefault(i => i.ProductId == productId);
        if (existing == null)
        {
            cart.Items.Add(new CartItemVm
            {
                ProductId = productId,
                Name = product.Name,
                Price = product.Price,
                Quantity = quantity,
                LineTotal = product.Price * quantity
            });
        }
        else
        {
            existing.Quantity += quantity;
            existing.LineTotal = existing.Quantity * existing.Price;
        }

        _cartSession.SaveCart(cart);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult Update(int productId, int quantity)
    {
        var cart = _cartSession.GetCart();
        var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
        if (item != null)
        {
            item.Quantity = Math.Max(1, quantity);
            item.LineTotal = item.Quantity * item.Price;
            _cartSession.SaveCart(cart);
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult Remove(int productId)
    {
        var cart = _cartSession.GetCart();
        cart.Items = cart.Items.Where(i => i.ProductId != productId).ToList();
        _cartSession.SaveCart(cart);
        return RedirectToAction(nameof(Index));
    }
}
