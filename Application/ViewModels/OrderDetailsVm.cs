using EcommerceIti.Domain.Entities;

namespace EcommerceIti.Application.ViewModels;

public class OrderDetailsVm
{
    public int OrderId { get; set; }
    public string OrderNumber { get; set; } = null!;
    public OrderStatus Status { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string ShippingTo { get; set; } = null!;
    public IList<OrderItemVm> Items { get; set; } = new List<OrderItemVm>();
}
