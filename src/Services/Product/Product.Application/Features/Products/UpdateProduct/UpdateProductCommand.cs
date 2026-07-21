using Product.Application.Common.Results;
using MediatR;

namespace Product.Application.Features.Products.UpdateProduct;

public sealed record UpdateProductCommand(
    Guid ProductId,
    string Name,
    string? Description,
    decimal Price,
    int StockQuantity,
    Guid CategoryId
) : IRequest<Result<UpdateProductResponse>>;