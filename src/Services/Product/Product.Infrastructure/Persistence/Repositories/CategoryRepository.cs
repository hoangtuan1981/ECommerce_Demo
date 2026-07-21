using Microsoft.EntityFrameworkCore;
using Product.Application.Common.Persistence;
using Product.Domain.Entities;

namespace Product.Infrastructure.Persistence.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ProductDbContext _context;

    public CategoryRepository(
       ProductDbContext context)
    {
        _context = context;
    }
    public async Task<Category?> GetByIdAsync(
    Guid id,
    CancellationToken cancellationToken)
    {
        return await _context.Categories
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }
}
