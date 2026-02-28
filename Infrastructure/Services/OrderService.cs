using EcommerceIti.Application.Interfaces;
using EcommerceIti.Application.ViewModels;
using EcommerceIti.Domain.Entities;
using EcommerceIti.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EcommerceIti.Infrastructure.Services;

public class OrderService : IOrderService
{
    private readonly MazadContext _context;

    public OrderService(MazadContext context)
    {
        _context = context;
    }

    public async Task<int> CheckoutAsync(string userId, int addressId, CartVm cart, CancellationToken cancellationToken = default)
    {
        if (cart.Items.Count == 0)
        {
            throw new InvalidOperationException("Cart is empty");
        }

        var address = await _context.Addresses
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.AddressId == addressId && a.UserId == userId, cancellationToken);
        if (address == null)
        {
            throw new InvalidOperationException("Address not found");
        }

        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        var productIds = cart.Items.Select(i => i.ProductId).ToList();
        var products = await _context.Products
            .Where(p => productIds.Contains(p.ProductId))
            .ToDictionaryAsync(p => p.ProductId, cancellationToken);

        foreach (var item in cart.Items)
        {
            if (!products.TryGetValue(item.ProductId, out var product))
            {
                throw new InvalidOperationException($"Product {item.ProductId} not found");
            }

            if (product.StockQuantity < item.Quantity)
            {
                throw new InvalidOperationException($"Insufficient stock for {product.Name}");
            }
        }

        var order = new Order
        {
            UserId = userId,
            ShippingAddressId = addressId,
            OrderDate = DateTime.UtcNow,
            Status = OrderStatus.Pending,
            OrderNumber = GenerateOrderNumber(),
            TotalAmount = 0m
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync(cancellationToken);

        foreach (var item in cart.Items)
        {
            var product = products[item.ProductId];
            product.StockQuantity -= item.Quantity;

            var orderItem = new OrderItem
            {
                OrderId = order.OrderId,
                ProductId = product.ProductId,
                UnitPrice = product.Price,
                Quantity = item.Quantity,
                LineTotal = product.Price * item.Quantity
            };

            order.TotalAmount += orderItem.LineTotal;
            _context.OrderItems.Add(orderItem);
        }

        await _context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return order.OrderId;
    }

    public async Task<IList<OrderSummaryVm>> GetOrdersAsync(string userId)
    {
        return await _context.Orders
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.OrderDate)
            .Select(o => new OrderSummaryVm
            {
                OrderId = o.OrderId,
                OrderNumber = o.OrderNumber,
                OrderDate = o.OrderDate,
                Status = o.Status,
                TotalAmount = o.TotalAmount
            })
            .ToListAsync();
    }

    public async Task<IList<OrderSummaryVm>> GetAllOrdersAsync()
    {
        return await _context.Orders
            .OrderByDescending(o => o.OrderDate)
            .Select(o => new OrderSummaryVm
            {
                OrderId = o.OrderId,
                OrderNumber = o.OrderNumber,
                OrderDate = o.OrderDate,
                Status = o.Status,
                TotalAmount = o.TotalAmount
            })
            .ToListAsync();
    }

    public async Task<OrderDetailsVm?> GetOrderAsync(int orderId, string? userId = null, bool asAdmin = false)
    {
        var query = _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .Include(o => o.ShippingAddress)
            .AsQueryable();

        if (!asAdmin && !string.IsNullOrWhiteSpace(userId))
        {
            query = query.Where(o => o.UserId == userId);
        }

        var order = await query.FirstOrDefaultAsync(o => o.OrderId == orderId);
        if (order == null)
        {
            return null;
        }

        return new OrderDetailsVm
        {
            OrderId = order.OrderId,
            OrderNumber = order.OrderNumber,
            Status = order.Status,
            OrderDate = order.OrderDate,
            TotalAmount = order.TotalAmount,
            ShippingTo = $"{order.ShippingAddress.Street}, {order.ShippingAddress.City}, {order.ShippingAddress.Country} {order.ShippingAddress.Zip}",
            Items = order.OrderItems.Select(oi => new OrderItemVm
            {
                ProductName = oi.Product.Name,
                Quantity = oi.Quantity,
                UnitPrice = oi.UnitPrice,
                LineTotal = oi.LineTotal
            }).ToList()
        };
    }

    public async Task UpdateStatusAsync(int orderId, OrderStatus status)
    {
        var order = await _context.Orders.FindAsync(orderId);
        if (order == null)
        {
            throw new InvalidOperationException("Order not found");
        }

        order.Status = status;
        await _context.SaveChangesAsync();
    }

    private static string GenerateOrderNumber() => $"ORD-{DateTime.UtcNow:yyyyMMddHHmmssfff}-{Guid.NewGuid().ToString("N")[..6]}";
}
