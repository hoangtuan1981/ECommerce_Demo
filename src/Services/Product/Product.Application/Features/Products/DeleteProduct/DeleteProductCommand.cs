using Product.Application.Common.Results;
using MediatR;

namespace Product.Application.Features.Products.DeleteProduct;

public sealed record DeleteProductCommand(
    Guid ProductId
) : IRequest<Result<DeleteProductResponse>>;