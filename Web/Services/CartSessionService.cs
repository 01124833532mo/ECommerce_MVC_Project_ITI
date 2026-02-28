using System.Text.Json;
using EcommerceIti.Application.ViewModels;

namespace EcommerceIti.Web.Services;

public class CartSessionService
{
    private const string CartKey = "CART";
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CartSessionService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public CartVm GetCart()
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        if (session == null)
        {
            return new CartVm();
        }

        var data = session.GetString(CartKey);
        if (string.IsNullOrEmpty(data))
        {
            return new CartVm();
        }

        return JsonSerializer.Deserialize<CartVm>(data) ?? new CartVm();
    }

    public void SaveCart(CartVm cart)
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        if (session == null)
        {
            return;
        }

        var data = JsonSerializer.Serialize(cart);
        session.SetString(CartKey, data);
    }

    public void Clear()
    {
        _httpContextAccessor.HttpContext?.Session?.Remove(CartKey);
    }
}
