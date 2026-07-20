using Product.Domain.Common;

namespace Product.Domain.Entities;

public class ProductImage : Entity
{
    public Guid ProductId { get; private set; }

    public string ImageUrl { get; private set; }

    public bool IsDefault { get; private set; }
}