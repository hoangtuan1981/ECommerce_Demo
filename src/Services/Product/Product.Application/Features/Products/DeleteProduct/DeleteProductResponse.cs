using Product.Domain.Entities;

namespace Product.Application.Features.Products.DeleteProduct;

public sealed record DeleteProductResponse(
    Guid Id,
    ProductStatus Status
);