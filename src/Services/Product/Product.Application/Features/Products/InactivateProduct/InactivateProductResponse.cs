using Product.Domain.Entities;

namespace Product.Application.Features.Products.InactivateProduct;

public sealed record InactivateProductResponse(
    Guid Id,
    ProductStatus Status
);