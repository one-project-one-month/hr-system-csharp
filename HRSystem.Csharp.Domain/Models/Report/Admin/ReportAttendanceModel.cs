namespace HRSystem.Csharp.Domain.Models.Report.Admin;

public class ReportAttendanceModel
{
    public string EmployeeCode { get; set; } = null!;
    public string EmployeeName { get; set; } = null!;
    public DateTime AttendanceDate { get; set; }
    public DateTime CheckInTime { get; set; }
    public DateTime CheckOutTime { get; set; }
    public string WorkingHour { get; set; } = null!;
    public string Remark { get; set; } = null!;
}


public class EmployeeReportAttendanceModel
{
    public DateTime AttendanceDate { get; set; }
    public DateTime CheckInTime { get; set; }
    public DateTime CheckOutTime { get; set; }
    public string WorkingHour { get; set; } = null!;
    public string Remark { get; set; } = null!;
}