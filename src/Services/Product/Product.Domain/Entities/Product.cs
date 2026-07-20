using Product.Domain.Common;

namespace Product.Domain.Entities;

public class Product : AggregateRoot
{
    public string ProductCode { get; private set; }
    public string ProductName { get; private set; }
    public Guid CategoryId { get; private set; }

    public decimal Price { get; private set; }

    public string Description { get; private set; }

    public int Quantity { get; private set; }

    public bool IsDeleted { get; private set; }

    public ProductStatus Status { get; private set; }

    public ICollection<ProductVariant> Variants { get; private set; }

    public ICollection<ProductImage> Images { get; private set; }
}
