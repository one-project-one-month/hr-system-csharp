namespace HRSystem.Csharp.Domain.Models.EmployeeAttendance;

public class EmployeeAttendanceRequestModel
{
    public string? EmployeeCode { get; set; }
    public string? CheckInStatus { get; set; }
    public string? Latitude { get; set; }
    public string? Longitude { get; set; }
    public string? Remark { get; set; }
}
