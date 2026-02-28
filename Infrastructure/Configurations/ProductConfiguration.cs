using EcommerceIti.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcommerceIti.Infrastructure.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(p => p.Name).HasMaxLength(200).IsRequired();
        builder.Property(p => p.SKU).HasMaxLength(100).IsRequired();
        builder.HasIndex(p => p.SKU).IsUnique();
        builder.Property(p => p.Price).HasColumnType("decimal(18,2)");
        builder.Property(p => p.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
