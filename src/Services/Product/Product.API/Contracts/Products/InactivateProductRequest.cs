namespace Product.API.Contracts.Products;

public sealed record InactivateProductRequest(
    Guid ProductId
);