namespace EcommerceIti.Domain.Entities;

public class Address
{
    public int AddressId { get; set; }
    public string UserId { get; set; } = null!;
    public string Country { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Street { get; set; } = null!;
    public string Zip { get; set; } = null!;
    public bool IsDefault { get; set; }

    public AppUser User { get; set; } = null!;
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
