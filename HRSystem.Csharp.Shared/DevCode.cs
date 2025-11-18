using HRSystem.Csharp.Shared.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace HRSystem.Csharp.Shared;

public static class DevCode
{
    private static long MaxFileSize = 5 * 1024 * 1024;
    public static string GenerateNewUlid()
    {
        return Ulid.NewUlid().ToString()!;
    }
    
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

    #region File Upload

    public static async Task<List<FileUploadData>> UploadFilesAsync(
        this EnumDirectory directory,
        IEnumerable<IFormFile> files)
    {
        if (files == null || !files.Any())
        {
            throw new Exception("Error: No files were uploaded.");
        }

        var wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        //var wwwrootPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, EnumDirectory.wwwroot.ToString());
        var directoryPath = Path.Combine(wwwrootPath, directory.ToString());
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        var uploadedFiles = new List<FileUploadData>();

        foreach (var file in files)
        {
            if (file == null || file.Length == 0)
            {
                throw new Exception($"Error: One or more files are empty.");
            }

            if (file.Length > MaxFileSize)
            {
                throw new Exception($"Error: '{file.FileName}' exceeds {MaxFileSize / (1024 * 1024)}MB limit.");
            }

            var fileName = GenerateNewUlid() + ".jpg";
            var filePath = Path.Combine(directoryPath, fileName).Replace("\\", "/");
            var relativePath = Path.Combine(directory.ToString(), fileName).Replace("\\", "/");
            try
            {
                using var fileStream2 = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(fileStream2);

                uploadedFiles.Add(new FileUploadData
                {
                    FilePath = relativePath,
                    FileName = fileName,
                });
            }
            catch (Exception ex)
            {
                foreach (var uploadedFile in uploadedFiles)
                {
                    try
                    {
                        File.Delete(uploadedFile.FilePath);
                    }
                    catch
                    {
                        throw;
                    }
                }
                throw new Exception($"Error uploading files: {ex.Message}");
            }
        }

        return uploadedFiles;
    }

    #endregion
}