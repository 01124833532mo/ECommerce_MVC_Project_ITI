using EcommerceIti.Application.ViewModels;

namespace EcommerceIti.Application.Interfaces;

public interface IAddressService
{
    Task<IList<AddressVm>> GetForUserAsync(string userId);
    Task<int> CreateAsync(string userId, AddressVm vm);
}
