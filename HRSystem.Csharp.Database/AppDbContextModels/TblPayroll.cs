using System;
using System.Collections.Generic;

namespace HRSystem.Csharp.Database.AppDbContextModels;

public partial class TblPayroll
{
    public Guid PayrollId { get; set; }

    public string? PayrollCode { get; set; }

    public string? EmployeeCode { get; set; }

    public DateTime? PayrollDate { get; set; }

    public string? Status { get; set; }

    public int? TotalWorkingHour { get; set; }

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
}
