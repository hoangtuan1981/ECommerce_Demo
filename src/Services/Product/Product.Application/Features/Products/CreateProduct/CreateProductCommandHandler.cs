using Product.Application.Common.Results;
using MediatR;
using Product.Application.Common.Persistence;
using Product.Domain.Entities;

namespace Product.Application.Features.Products.CreateProduct;

public sealed class CreateProductCommandHandler
    : IRequestHandler<
        CreateProductCommand,
        Result<CreateProductResponse>>
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductCommandHandler(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CreateProductResponse>> Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(
            request.CategoryId,
            cancellationToken);

        if (category is null)
        {
            return Result<CreateProductResponse>.Failure(
                Error.NotFound(
                    "Category.NotFound",
                    "Category not found."));
        }

        var product = new Domain.Entities.Product
        {
            //Id = Guid.NewGuid(),
            //Name = request.Name,
            //Description = request.Description,
            //Price = request.Price,
            //StockQuantity = request.StockQuantity,
            //CategoryId = request.CategoryId,
            //Status = ProductStatus.Active
        };

        await _productRepository.AddAsync(
            product,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        // TODO
        return new CreateProductResponse(
           Guid.NewGuid(),
            string.Empty,
            -15,
            -10
            //product.Id,
            //product.Name,
            //product.Price,
            //product.StockQuantity
            );
    }
}