namespace HRSystem.Csharp.Domain.Models.Report;

public class ReportResponseModel
{
    public object ReportData { get; set; }
    public int TotalRecords { get; set; }
}

public class PaginatedResponse<T> : ReportResponseModel
{
    public List<T> Data { get; set; } = new List<T>();
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
}