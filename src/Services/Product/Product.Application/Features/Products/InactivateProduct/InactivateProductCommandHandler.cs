using MediatR;
using Product.Application.Common.Persistence;
using Product.Application.Common.Results;
using Product.Application.Features.Products.CreateProduct;
using Product.Domain.Entities;

namespace Product.Application.Features.Products.InactivateProduct;

public sealed class InactivateProductCommandHandler
    : IRequestHandler<InactivateProductCommand,
        Result<InactivateProductResponse>>
{
    private readonly IProductRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public InactivateProductCommandHandler(
        IProductRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<InactivateProductResponse>> Handle(
        InactivateProductCommand request,
        CancellationToken cancellationToken)
    {
        var product = await _repository.GetByIdAsync(
            request.ProductId,
            cancellationToken);

        if (product is null)
        {
            return Result<InactivateProductResponse>.Failure(
                Error.NotFound(
                    "Product.NotFound",
                    "Product not found."));
        }

        // TODO
        //product.Status = ProductStatus.Inactive;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new InactivateProductResponse(
            product.Id,
            product.Status);
    }
}