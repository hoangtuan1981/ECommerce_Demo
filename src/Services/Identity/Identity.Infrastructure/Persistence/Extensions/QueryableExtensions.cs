using Identity.Application.Common.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Persistence.Extensions;


public static class PaginationExtensions
{
    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
        this IQueryable<T> query,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return PagedResult<T>.Create(
            items,
            pageNumber,
            pageSize,
            totalCount);
    }
}