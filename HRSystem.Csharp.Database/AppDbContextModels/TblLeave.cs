using System;
using System.Collections.Generic;

namespace HRSystem.Csharp.Database.AppDbContextModels;

public partial class TblLeave
{
    public string LeaveId { get; set; } = null!;

    public string EmployeeCode { get; set; } = null!;

    public string? LeaveType { get; set; }

    public string? Reason { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public decimal? TotalHours { get; set; }

    public bool? IsPaid { get; set; }

    public string? Status { get; set; }

    public string? ApprovedBy { get; set; }

    public DateTime? ApprovedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public bool? DeleteFlag { get; set; }
}
