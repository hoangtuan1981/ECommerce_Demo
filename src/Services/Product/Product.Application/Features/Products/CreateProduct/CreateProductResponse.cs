namespace Product.Application.Features.Products.CreateProduct;

public sealed record CreateProductResponse(
    Guid Id,
    string Name,
    decimal Price,
    int StockQuantity
);