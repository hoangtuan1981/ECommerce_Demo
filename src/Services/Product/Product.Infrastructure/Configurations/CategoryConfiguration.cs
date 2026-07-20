using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Product.Domain.Entities;

namespace Product.Infrastructure.Configurations;

public sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.CategoryCode)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.CategoryName)
            .HasMaxLength(200)
            .IsRequired();

        builder.HasIndex(x => x.CategoryCode)
            .IsUnique();
    }
}