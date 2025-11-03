namespace HRSystem.Csharp.Domain.Models.Common;

public class PaginationRequestModel
{
    public int PageNo { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}