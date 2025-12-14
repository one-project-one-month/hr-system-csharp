namespace HRSystem.Csharp.Domain.Models.AttendanceReports;

public class HRAttendanceOverviewReport
{
    public HRAttendanceOverviewReport()
    {
    }

    public Int32 Present { get; set; } = 0;
    public Int32 Absent { get; set; } = 0;
    public Int32 Late { get; set; } = 0;
    public Int32 EmpCount { get; set; } = 0;
}