namespace Product.API.Contracts.Products;

public sealed record ActivateProductRequest(
    Guid ProductId
);