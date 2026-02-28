using EcommerceIti.Domain.Entities;
using EcommerceIti.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EcommerceIti.Infrastructure.Seed;

public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var provider = scope.ServiceProvider;

        var context = provider.GetRequiredService<MazadContext>();
        await context.Database.MigrateAsync();

        var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = provider.GetRequiredService<UserManager<AppUser>>();

        var roles = new[] { "Admin", "Manager", "Support" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        var adminEmail = "admin@local";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new AppUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FullName = "Administrator",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(adminUser, "Admin@123");
            await userManager.AddToRolesAsync(adminUser, roles);
        }

        if (!context.Categories.Any())
        {
            var electronics = new Category { Name = "Electronics" };
            var fashion = new Category { Name = "Fashion" };
            var home = new Category { Name = "Home" };

            context.Categories.AddRange(electronics, fashion, home);
            await context.SaveChangesAsync();

            context.Products.AddRange(
                new Product { Name = "Headphones", SKU = "HD-100", Price = 79.99m, StockQuantity = 50, IsActive = true, CategoryId = electronics.CategoryId, CreatedAt = DateTime.UtcNow },
                new Product { Name = "Laptop", SKU = "LP-200", Price = 999.99m, StockQuantity = 20, IsActive = true, CategoryId = electronics.CategoryId, CreatedAt = DateTime.UtcNow },
                new Product { Name = "T-Shirt", SKU = "TS-300", Price = 19.99m, StockQuantity = 200, IsActive = true, CategoryId = fashion.CategoryId, CreatedAt = DateTime.UtcNow }
            );

            await context.SaveChangesAsync();
        }
    }
}
