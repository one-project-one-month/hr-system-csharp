namespace HRSystem.Csharp.Domain.Models.Report.Admin;

public class ReportBacklogModel
{
    public string TaskCode { get; set; } = null!;
    public string ProjectName { get; set; } = null!;
    public string AssignedTo { get; set; } = null!;
    public string StartDate { get; set; } = null!;
    public string EndDate { get; set; } = null!;
    public string Status { get; set; } = null!;
    public string WorkingHour { get; set; } = null!;
    public string CreatedAt { get; set; } = null!;
    public string CreatedBy { get; set; } = null!;
}