using Microsoft.AspNetCore.Identity;

namespace EcommerceIti.Domain.Entities;

public class AppUser : IdentityUser
{
    public string? FullName { get; set; }

    public ICollection<Order> Orders { get; set; } = new List<Order>();
    public ICollection<Address> Addresses { get; set; } = new List<Address>();
}
