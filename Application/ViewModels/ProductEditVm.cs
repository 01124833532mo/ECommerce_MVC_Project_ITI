namespace EcommerceIti.Application.ViewModels;

public class ProductEditVm
{
    public int? ProductId { get; set; }
    public int CategoryId { get; set; }
    public string Name { get; set; } = null!;
    public string SKU { get; set; } = null!;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public bool IsActive { get; set; }
}
