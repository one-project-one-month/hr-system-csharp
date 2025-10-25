namespace HRSystem.Csharp.Domain.Models.Attendance;

public class AttendanceCreateRequestModel
{
    public string? EmployeeCode { get; set; }

    public DateTime? AttendanceDate { get; set; }

    public DateTime? CheckInTime { get; set; }

    public string? CheckInLocation { get; set; }

    public DateTime? CheckOutTime { get; set; }

    public string? CheckOutLocation { get; set; }

    public string? Remark { get; set; }
}