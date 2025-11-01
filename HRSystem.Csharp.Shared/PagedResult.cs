namespace HRSystem.Csharp.Shared;

public class PagedResult<T>
{
    public List<T>? Items { get; set; }
    public int TotalCount { get; set; }
    public int PageNo { get; set; }
    public int PageSize { get; set; }
}