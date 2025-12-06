using HRSystem.Csharp.Shared.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace HRSystem.Csharp.Shared;

public static class DevCode
{
    private static readonly long MaxFileSize = 5 * 1024 * 1024;

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
        logger.LogError("File Name - {FileName} | Method Name - {MethodName} | Result - {Result}",
            Path.GetFileName(filePath),
            methodName,
            str?.ToString() ?? "[null]");
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

    public static async Task<List<FileUploadData>> UploadFilesAsync(this EnumDirectory directory, IEnumerable<IFormFile> files)
    {
        if (files == null || !files.Any())
        {
            throw new Exception("Error: No files were uploaded.");
        }

        var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), directory.ToString());
        if (!Directory.Exists(uploadPath))
        {
            Directory.CreateDirectory(uploadPath);
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
            var filePath = Path.Combine(uploadPath, fileName);
            var savePath = Path.Combine(directory.ToString(), fileName);
            try
            {
                using var fileStream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(fileStream);

                uploadedFiles.Add(new FileUploadData
                {
                    FilePath = savePath,
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
                        // throw;
                    }
                }
                throw new Exception($"Error uploading files: {ex.Message}");
            }
        }

        return uploadedFiles;
    }

    #endregion

    #region Base64 Extenstion

    public static async Task<string> GetBase64FromFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new Exception($"File not found at path: {filePath}");
        }

        byte[] fileBytes = await File.ReadAllBytesAsync(filePath);
        return Convert.ToBase64String(fileBytes);
    }

    #endregion
}

public class FileUploadData
{
    public string FilePath { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
}