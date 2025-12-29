namespace HRSystem.Csharp.Domain.Models.AttendanceReports;

public class AdminAttendanceOverviewReport
{
    public AdminAttendanceOverviewReport()
    {
    }
    public String Date { get; set; } = String.Empty;
    public Int32 Present { get; set; } = 0;
    public Int32 Absent { get; set; } = 0;
    public Int32 HalfDayLeave { get; set; } = 0;
    public Int32 EmpCount { get; set; } = 0;
    public Int32 ProjCount { get; set; } = 0;
    public Int32 TdyAbsent { get; set; } = 0;
}