using Microsoft.EntityFrameworkCore;
using Product.Application.Common.Persistence;
using Product.Domain.Entities;

namespace Product.Infrastructure;

public class ProductDbContext : DbContext, IUnitOfWork
{
    public ProductDbContext(DbContextOptions<ProductDbContext> options)
        : base(options)
    {
    }
    public DbSet<Domain.Entities.Product> Products { get; set; }

    public DbSet<Category> Categories { get; set; }

    public DbSet<ProductVariant> ProductVariants { get; set; }

    public DbSet<ProductImage> ProductImages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}