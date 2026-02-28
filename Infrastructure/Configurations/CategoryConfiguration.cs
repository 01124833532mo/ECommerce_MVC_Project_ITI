using EcommerceIti.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcommerceIti.Infrastructure.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.Property(c => c.Name).HasMaxLength(150).IsRequired();
        builder.HasIndex(c => c.Name).IsUnique();

        builder.HasOne(c => c.ParentCategory)
            .WithMany(c => c.Children)
            .HasForeignKey(c => c.ParentCategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
