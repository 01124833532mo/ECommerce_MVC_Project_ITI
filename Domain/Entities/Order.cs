namespace EcommerceIti.Domain.Entities;

public class Order
{
    public int OrderId { get; set; }
    public string OrderNumber { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public int ShippingAddressId { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }

    public AppUser User { get; set; } = null!;
    public Address ShippingAddress { get; set; } = null!;
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
