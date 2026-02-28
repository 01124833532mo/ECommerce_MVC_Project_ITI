namespace EcommerceIti.Application.ViewModels;

public class CartVm
{
    public IList<CartItemVm> Items { get; set; } = new List<CartItemVm>();
    public decimal Subtotal => Items.Sum(i => i.LineTotal);
    public decimal Total => Subtotal; // Extend later for shipping/taxes
}
