using Product.Application.Common.Results;
using MediatR;
using Product.Application.Common.Persistence;
using Product.Domain.Entities;

namespace Product.Application.Features.Products.ActivateProduct;

public sealed class ActivateProductCommandHandler
    : IRequestHandler<ActivateProductCommand,
        Result<ActivateProductResponse>>
{
    private readonly IProductRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public ActivateProductCommandHandler(
        IProductRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ActivateProductResponse>> Handle(
        ActivateProductCommand request,
        CancellationToken cancellationToken)
    {
        var product = await _repository.GetByIdAsync(
            request.ProductId,
            cancellationToken);

        if (product is null)
        {
            return Result<ActivateProductResponse>.Failure(
                Error.NotFound(
                    "Product.NotFound",
                    "Product not found."));
        }

        //TODO
        //product.Status = ProductStatus.Active;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new ActivateProductResponse(
            product.Id,
            product.Status);
    }
}