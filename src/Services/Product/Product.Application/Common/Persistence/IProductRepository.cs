

namespace Product.Application.Common.Persistence;

public interface IProductRepository
{
    Task<Domain.Entities.Product?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task AddAsync(
        Domain.Entities.Product product,
        CancellationToken cancellationToken);

    Task<bool> ExistsByNameAsync(
        string name,
        CancellationToken cancellationToken);

    IQueryable<Domain.Entities.Product> Query();
}