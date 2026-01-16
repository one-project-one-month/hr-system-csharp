namespace HRSystem.Csharp.Domain.Models.EmployeeAttendance;

public class AttendanceRequestModel
{
    public string AttendanceCode { get; set; } = string.Empty;
    public string EmployeeCode { get; set; } = string.Empty;
    public DateTime CheckInTime { get; set; }
    public string CheckInLocation { get; set; } = string.Empty;
    public DateTime? CheckOutTime { get; set; }
    public string CheckOutLocation { get; set; } = string.Empty;
    public TimeSpan? WorkingHours { get; set; }
    public int? HourLateFlag { get; set; }
    public int? HalfDayFlag { get; set; }
    public int? FullDayFlag { get; set; }
    public bool? IsSavedLocation { get; set; }
    public string Remark { get; set; } = string.Empty;
}