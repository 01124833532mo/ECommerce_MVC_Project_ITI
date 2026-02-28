namespace EcommerceIti.Domain.Entities;

public class Product
{
    public int ProductId { get; set; }
    public int CategoryId { get; set; }
    public string Name { get; set; } = null!;
    public string SKU { get; set; } = null!;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    public Category Category { get; set; } = null!;
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
