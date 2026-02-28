using EcommerceIti.Domain.Entities;

namespace EcommerceIti.Application.ViewModels;

public class OrderSummaryVm
{
    public int OrderId { get; set; }
    public string OrderNumber { get; set; } = null!;
    public OrderStatus Status { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
}
