namespace Product.Application.Features.Products.UpdateProduct;

public sealed record UpdateProductResponse(
    Guid Id,
    string Name,
    decimal Price,
    int StockQuantity
);