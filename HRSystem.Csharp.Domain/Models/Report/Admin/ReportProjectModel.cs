namespace HRSystem.Csharp.Domain.Models.Report.Admin;

public class ReportProjectModel
{
    public string ProjectCode { get; set; } = null!;
    public string ProjectName { get; set; } = null!;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string ProjectStatus { get; set; } = null!;
}