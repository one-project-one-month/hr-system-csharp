using System;
using System.Collections.Generic;

namespace HRSystem.Csharp.Database.AppDbContextModels;

public partial class TblPayroll
{
    public string PayrollId { get; set; } = null!;

    public string PayrollCode { get; set; } = null!;

    public string? EmployeeCode { get; set; }

    public string PayrollMonth { get; set; } = string.Empty;

    public string? Status { get; set; }

    public decimal? TotalWorkingHour { get; set; }

    public decimal? LeaveHour { get; set; }

    public decimal? ActualWorkingHour { get; set; }

    public decimal? BaseSalary { get; set; }

    public decimal? Allowance { get; set; }

    public decimal? GrossPay { get; set; }

    public decimal? Deduction { get; set; }

    public decimal? Tax { get; set; }

    public decimal? Bonus { get; set; }

    public decimal? NetPay { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public bool? DeleteFlag { get; set; }

    public string PayrollSummaryCode { get; set; } = null!;

    public DateTime PayrollDate { get; set; }
}
