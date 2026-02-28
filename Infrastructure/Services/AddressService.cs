using EcommerceIti.Application.Interfaces;
using EcommerceIti.Application.ViewModels;
using EcommerceIti.Domain.Entities;
using EcommerceIti.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EcommerceIti.Infrastructure.Services;

public class AddressService : IAddressService
{
    private readonly MazadContext _context;

    public AddressService(MazadContext context)
    {
        _context = context;
    }

    public async Task<IList<AddressVm>> GetForUserAsync(string userId)
    {
        return await _context.Addresses
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.IsDefault)
            .ThenBy(a => a.AddressId)
            .Select(a => new AddressVm
            {
                AddressId = a.AddressId,
                Country = a.Country,
                City = a.City,
                Street = a.Street,
                Zip = a.Zip,
                IsDefault = a.IsDefault
            })
            .ToListAsync();
    }

    public async Task<int> CreateAsync(string userId, AddressVm vm)
    {
        var address = new Address
        {
            UserId = userId,
            Country = vm.Country,
            City = vm.City,
            Street = vm.Street,
            Zip = vm.Zip,
            IsDefault = vm.IsDefault
        };

        if (vm.IsDefault)
        {
            await _context.Addresses
                .Where(a => a.UserId == userId)
                .ExecuteUpdateAsync(s => s.SetProperty(a => a.IsDefault, false));
        }

        _context.Addresses.Add(address);
        await _context.SaveChangesAsync();
        return address.AddressId;
    }
}
