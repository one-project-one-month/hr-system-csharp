namespace HRSystem.Csharp.Domain.Models.AttendanceReports;

public class StaffAttendanceOverviewReport
{
    public List<StaffAttendanceOverviewReportModel> staffAttendanceOverview { get; set; } =
        new List<StaffAttendanceOverviewReportModel>();
}

public class StaffAttendanceOverviewReportModel
{
    public string month { get; set; } = string.Empty;
    public int present { get; set; } = 0;
    public int late { get; set; } = 0;
}