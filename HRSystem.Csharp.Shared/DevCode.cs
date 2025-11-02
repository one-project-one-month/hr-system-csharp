using Microsoft.EntityFrameworkCore;

namespace HRSystem.Csharp.Shared;

public static class DevCode
{
    public static string GenerateNewUlid()
    {
        return Ulid.NewUlid().ToString()!;
    }
    
    public static async Task<PagedResult<T>> GetPagedResultAsync<T>(this IQueryable<T> query, int pageNo, int pageSize)
    {
        var totalCount = await query.CountAsync();

        List<T> pagedData;

        if (pageNo <= 0 || pageSize <= 0)
        {
            pagedData = await query.ToListAsync();
        }
        else
        {
            pagedData = await query
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        return new PagedResult<T>
        {
            Items = pagedData,
            TotalCount = totalCount,
            PageNo = pageNo,
            PageSize = pageSize
        };
    }
}