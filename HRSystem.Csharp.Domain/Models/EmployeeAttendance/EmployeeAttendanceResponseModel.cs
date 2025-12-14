namespace HRSystem.Csharp.Domain.Models.EmployeeAttendance;

public class EmployeeAttendanceResponseModel
{
    public DateTime? AttendanceDate { get; set; }
    public bool? IsCheckIn { get; set; }    
    public bool? IsCheckOut { get; set; }
    public string? CheckInTime { get; set; }
    public string? CheckOutTime { get; set; }
}
