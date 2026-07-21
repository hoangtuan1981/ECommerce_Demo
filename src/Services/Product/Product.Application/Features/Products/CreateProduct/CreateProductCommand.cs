using Product.Application.Common.Results;
using MediatR;

namespace Product.Application.Features.Products.CreateProduct;

public sealed record CreateProductCommand(
    string Name,
    string? Description,
    decimal Price,
    int StockQuantity,
    Guid CategoryId
) : IRequest<Result<CreateProductResponse>>;