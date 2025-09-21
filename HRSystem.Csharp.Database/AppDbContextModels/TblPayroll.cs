using System;
using System.Collections.Generic;

namespace HRSystem.Csharp.Database.AppDbContextModels;

public partial class TblPayroll
{
    public string PayrollId { get; set; } = null!;

    public string PayrollCode { get; set; } = null!;

    public decimal BaseSalary { get; set; }

    public decimal? AttendenceDeduction { get; set; }

    public decimal? OvertimePay { get; set; }

    public decimal? Tax { get; set; }

    public string EmployeeCode { get; set; } = null!;

    public decimal? Bonus { get; set; }

    public decimal? Allowance { get; set; }

    public decimal? TotalOvertimeHours { get; set; }

    public decimal? TotalLeaveDays { get; set; }

    public decimal TotalSalary { get; set; }

    public DateTime PaymentDate { get; set; }

    public string PaymentStatus { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public bool? DeleteFlag { get; set; }
}
