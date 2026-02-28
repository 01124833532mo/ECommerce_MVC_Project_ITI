namespace EcommerceIti.Application.ViewModels;

public class CheckoutVm
{
    public int? SelectedAddressId { get; set; }
    public IList<AddressVm> Addresses { get; set; } = new List<AddressVm>();
    public CartVm Cart { get; set; } = new();
}
