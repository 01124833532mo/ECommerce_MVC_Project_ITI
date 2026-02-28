using EcommerceIti.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcommerceIti.Infrastructure.Configurations;

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.Property(a => a.Country).HasMaxLength(100).IsRequired();
        builder.Property(a => a.City).HasMaxLength(100).IsRequired();
        builder.Property(a => a.Street).HasMaxLength(200).IsRequired();
        builder.Property(a => a.Zip).HasMaxLength(20).IsRequired();

        builder.HasOne(a => a.User)
            .WithMany(u => u.Addresses)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
