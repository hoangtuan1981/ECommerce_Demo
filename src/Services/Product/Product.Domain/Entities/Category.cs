using Product.Domain.Common;

namespace Product.Domain.Entities;

public class Category : AggregateRoot
{
    public string CategoryCode { get; private set; }

    public string CategoryName { get; private set; }
}
