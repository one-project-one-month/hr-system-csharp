using System;
using System.Collections.Generic;

namespace HRSystem.Csharp.Database.AppDbContextModels;

public partial class TblAttendance
{
    public string AttendanceId { get; set; } = null!;

    public DateOnly Date { get; set; }

    public string EmployeeShiftCode { get; set; } = null!;

    public TimeOnly CheckInTime { get; set; }

    public TimeOnly? CheckOutTime { get; set; }

    public string CheckInLocation { get; set; } = null!;

    public string? CheckOutLocation { get; set; }

    public decimal? WorkingHour { get; set; }

    public bool? CheckInWarningFlag { get; set; }

    public bool? CheckOutWarningFlag { get; set; }

    public string? Status { get; set; }

    public string? Reason { get; set; }

    public decimal? OvertimeHour { get; set; }

    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }
}
