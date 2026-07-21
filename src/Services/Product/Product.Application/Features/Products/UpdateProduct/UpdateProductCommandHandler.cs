using Product.Application.Common.Results;
using MediatR;
using Product.Application.Common.Persistence;

namespace Product.Application.Features.Products.UpdateProduct;

public sealed class UpdateProductCommandHandler
    : IRequestHandler<
        UpdateProductCommand,
        Result<UpdateProductResponse>>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductCommandHandler(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<UpdateProductResponse>> Handle(
        UpdateProductCommand request,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(
            request.ProductId,
            cancellationToken);

        if (product is null)
        {
            return Result<UpdateProductResponse>.Failure(
                Error.NotFound(
                    "Product.NotFound",
                    "Product not found."));
        }

        // TODO
        //product.Name = request.Name;
        //product.Description = request.Description;
        //product.Price = request.Price;
        //product.StockQuantity = request.StockQuantity;
        //product.CategoryId = request.CategoryId;

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return new UpdateProductResponse(
            product.Id,
            product.ProductName,
            product.Price,
            product.Quantity
            );
    }
}