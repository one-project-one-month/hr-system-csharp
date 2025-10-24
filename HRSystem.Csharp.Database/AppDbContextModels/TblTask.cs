using System;
using System.Collections.Generic;

namespace HRSystem.Csharp.Database.AppDbContextModels;

public partial class TblTask
{
    public string TaskId { get; set; } = null!;

    public string TaskCode { get; set; } = null!;

    public string? EmployeeCode { get; set; }

    public string? ProjectCode { get; set; }

    public string? TaskName { get; set; }

    public string? TaskDescription { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? TaskStatus { get; set; }

    public decimal? WorkingHour { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public bool? DeleteFlag { get; set; }
}
