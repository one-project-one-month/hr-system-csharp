using HRSystem.Csharp.Domain.Models.Common;

namespace HRSystem.Csharp.Domain.Models.Report;

public class ReportRequestModel : PaginationRequestModel
{
    public string ReportType { get; set; } = null!;
    public DateTime FromDate { get; set; } = DateTime.UtcNow;
    public DateTime ToDate { get; set; } = DateTime.UtcNow;
    public string Item { get; set; }
}