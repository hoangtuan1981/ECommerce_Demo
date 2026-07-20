using Product.Domain.Common;

namespace Product.Domain.Entities;

public class ProductVariant : Entity
{
    public Guid ProductId { get; private set; }

    public string Color { get; private set; }

    public string Size { get; private set; }

    public int Quantity { get; private set; }
}