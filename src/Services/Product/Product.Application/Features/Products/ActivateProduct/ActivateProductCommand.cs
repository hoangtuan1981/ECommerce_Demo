using Product.Application.Common.Results;
using MediatR;

namespace Product.Application.Features.Products.ActivateProduct;

public sealed record ActivateProductCommand(
    Guid ProductId
) : IRequest<Result<ActivateProductResponse>>;