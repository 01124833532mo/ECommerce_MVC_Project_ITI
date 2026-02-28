using EcommerceIti.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcommerceIti.Infrastructure.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(o => o.OrderNumber).HasMaxLength(32).IsRequired();
        builder.HasIndex(o => o.OrderNumber).IsUnique();
        builder.Property(o => o.TotalAmount).HasColumnType("decimal(18,2)");

        builder.HasOne(o => o.User)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(o => o.ShippingAddress)
            .WithMany(a => a.Orders)
            .HasForeignKey(o => o.ShippingAddressId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
