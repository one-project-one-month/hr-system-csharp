using System;
using System.Collections.Generic;

namespace HRSystem.Csharp.Database.AppDbContextModels;

public partial class TblLeave
{
    public string LeaveId { get; set; } = null!;

    public string EmployeeCode { get; set; } = null!;

    public string LeaveType { get; set; } = null!;

    public string Reason { get; set; } = null!;

    public DateOnly FromDate { get; set; }

    public DateOnly ToDate { get; set; }

    public decimal? TotalHours { get; set; }

    public bool? IsPaid { get; set; }

    public string Status { get; set; } = null!;

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = null!;

    public bool DeleteFlag { get; set; }

    public string FullOrHalf { get; set; } = null!;

    public string LeaveCode { get; set; } = null!;
}
