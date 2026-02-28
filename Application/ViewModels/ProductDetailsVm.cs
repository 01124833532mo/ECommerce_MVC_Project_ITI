namespace EcommerceIti.Application.ViewModels;

public class ProductDetailsVm
{
    public int ProductId { get; set; }
    public string Name { get; set; } = null!;
    public string SKU { get; set; } = null!;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CategoryName { get; set; } = null!;
}
