using System;
using System.Collections.Generic;

namespace HRSystem.Csharp.Database.AppDbContextModels;

public partial class TblPayroll
{
    public string PayrollId { get; set; } = null!;

    public string PayrollCode { get; set; } = null!;

    public string? EmployeeCode { get; set; }

    public DateTime? PayrollDate { get; set; }

    public string? PayrollStatus { get; set; }

    public decimal? BaseSalary { get; set; }

    public decimal? Allowance { get; set; }

    public int? TotalWorkingHour { get; set; }

    public decimal? LeaveHour { get; set; }

    public decimal? ActualWorkingHour { get; set; }

    public decimal? Deduction { get; set; }

    public decimal? TotalPayroll { get; set; }

    public decimal? Tax { get; set; }

    public decimal? Bonus { get; set; }

    public decimal? GrandTotalPayroll { get; set; }


    public bool? DeleteFlag { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
