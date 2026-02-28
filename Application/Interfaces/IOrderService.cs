using EcommerceIti.Application.ViewModels;
using EcommerceIti.Domain.Entities;

namespace EcommerceIti.Application.Interfaces;

public interface IOrderService
{
    Task<int> CheckoutAsync(string userId, int addressId, CartVm cart, CancellationToken cancellationToken = default);
    Task<IList<OrderSummaryVm>> GetOrdersAsync(string userId);
    Task<IList<OrderSummaryVm>> GetAllOrdersAsync();
    Task<OrderDetailsVm?> GetOrderAsync(int orderId, string? userId = null, bool asAdmin = false);
    Task UpdateStatusAsync(int orderId, OrderStatus status);
}
