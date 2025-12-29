namespace HRSystem.Csharp.Domain.Models.AttendanceReports;

public class AdminAttendanceOverviewReport
{
    public AdminAttendanceOverviewReport()
    {
    }

    public Int32 Present { get; set; } = 0;
    public Int32 Absent { get; set; } = 0;
    public Int32 HalfDayLeave { get; set; } = 0;
}