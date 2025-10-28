using System;
using System.Collections.Generic;

namespace HRSystem.Csharp.Database.AppDbContextModels;

public partial class TblAttendance
{
    public string AttendanceId { get; set; } = null!;

    public string AttendanceCode { get; set; } = null!;

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

    public bool? IsSavedLocation { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public bool? DeleteFlag { get; set; }
}
