using EcommerceIti.Application.Interfaces;
using EcommerceIti.Domain.Entities;
using EcommerceIti.Infrastructure.Data;
using EcommerceIti.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EcommerceIti.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MazadContext")
            ?? throw new InvalidOperationException("Connection string 'MazadContext' not found.");

        services.AddDbContext<MazadContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
            })
            .AddEntityFrameworkStores<MazadContext>()
            .AddDefaultTokenProviders()
            .AddDefaultUI();

        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IAddressService, AddressService>();

        return services;
    }
}
