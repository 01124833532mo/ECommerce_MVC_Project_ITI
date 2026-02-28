using EcommerceIti.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EcommerceIti.Infrastructure.Data;

public class MazadContext : IdentityDbContext<AppUser>
{
    public MazadContext(DbContextOptions<MazadContext> options) : base(options)
    {
    }

    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(MazadContext).Assembly);
    }
}
