using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Csharp.Domain.Models.Attendance;

public class AttendanceUpdateRequestModel
{
    public string? AttendanceCode { get; set; }

    public string? EmployeeCode { get; set; }

    public DateTime? AttendanceDate { get; set; }

    public DateTime? CheckInTime { get; set; }

    public string? CheckInLocation { get; set; }

    public DateTime? CheckOutTime { get; set; }

    public string? CheckOutLocation { get; set; }

    public decimal? WorkingHour { get; set; }

    public int? HourLateFlag { get; set; }

    public int? HalfDayFlag { get; set; }

    public int? FullDayFlag { get; set; }

    public string? Remark { get; set; }
}
