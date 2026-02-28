using EcommerceIti.Application.Interfaces;
using EcommerceIti.Application.ViewModels;
using EcommerceIti.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceIti.Web.Controllers;

[Authorize]
public class AddressesController : Controller
{
    private readonly IAddressService _addressService;

    public AddressesController(IAddressService addressService)
    {
        _addressService = addressService;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.GetUserId();
        var addresses = await _addressService.GetForUserAsync(userId);
        return View(addresses);
    }

    public IActionResult Create()
    {
        return View(new AddressVm { IsDefault = true });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AddressVm vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }
        var userId = User.GetUserId();
        await _addressService.CreateAsync(userId, vm);
        return RedirectToAction(nameof(Index));
    }
}
