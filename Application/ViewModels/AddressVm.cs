namespace EcommerceIti.Application.ViewModels;

public class AddressVm
{
    public int AddressId { get; set; }
    public string Country { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Street { get; set; } = null!;
    public string Zip { get; set; } = null!;
    public bool IsDefault { get; set; }
}
