using Product.Application.Common.Persistence;

namespace Product.Infrastructure.Persistence.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ProductDbContext _context;

    public ProductRepository(
           ProductDbContext context)
    {
        _context = context;
    }
    public Task AddAsync(Domain.Entities.Product product, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Domain.Entities.Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public IQueryable<Domain.Entities.Product> Query()
    {
        throw new NotImplementedException();
    }
}
