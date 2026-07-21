using Product.Application.Common.Results;
using MediatR;

namespace Product.Application.Features.Products.InactivateProduct;

public sealed record InactivateProductCommand(
    Guid ProductId
) : IRequest<Result<InactivateProductResponse>>;