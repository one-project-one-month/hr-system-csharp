using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace HRSystem.Csharp.Shared;

public static class DevCode
{
    public static string ToJson(this object obj)
    {
        return JsonConvert.SerializeObject(obj);
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

    public static void LogExceptionError(this ILogger logger,
        Exception ex,
        [CallerFilePath] string filePath = "",
        [CallerMemberName] string methodName = "")
    {
        var fileName = Path.GetFileName(filePath);
        var message = $"File Name - {fileName} | Method Name - {methodName} | Error - {ex.ToJson()}";
        logger.LogCustomError(message);
    }
    public static void LogCustomError(this ILogger logger,
        object? str,
        [CallerFilePath] string filePath = "",
        [CallerMemberName] string methodName = "")
    {
        var fileName = Path.GetFileName(filePath);
        var message =
            $"File Name - {fileName} | Method Name - {methodName} | Result - {(str is string ? str : str!.ToJson())}";
        logger.LogError(message);
    }

    public static bool IsValidEmail(this string email)
    {
        bool result = true;

        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            if (addr.Address != email)
            {
                result = false;
            }
        }
        catch
        {
            result = false;
        }

        return result;
    }
}