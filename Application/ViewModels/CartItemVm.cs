namespace EcommerceIti.Application.ViewModels;

public class CartItemVm
{
    public int ProductId { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal LineTotal { get; set; }
}
