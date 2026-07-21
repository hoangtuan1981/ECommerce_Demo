using Product.Domain.Entities;

namespace Product.Application.Common.Persistence;

public interface ICategoryRepository
{
    Task<Category?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken);
}