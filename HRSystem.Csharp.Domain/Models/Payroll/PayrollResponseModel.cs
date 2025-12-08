namespace HRSystem.Csharp.Domain.Models.Payroll;

/*public class PayrollResponseModel
{
    public string PayrollId { get; set; } = string.Empty;
    public string PayrollCode { get; set; } = string.Empty;
    public string EmployeeCode { get; set; } = string.Empty;
    public DateTime? PayrollDate { get; set; } // Use DateTime, not DateOnly
    public int? TotalWorkingHour { get; set; }
    public int? LeaveHour { get; set; }
    public int? ActualWorkingHour { get; set; }
    public decimal? BaseSalary { get; set; }
    public decimal? Bonus { get; set; }
    public decimal? GrossPay { get; set; }
    public decimal? Deduction { get; set; }
    public decimal? Tax { get; set; }
    public decimal? NetPay { get; set; }
    public string? Status { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
}*/
public class PayrollResponseModel
{
    public int PayrollSummaryId { get; set; }

    public string PayrollMonth { get; set; } = null!;

    public int TotalWorkingDays { get; set; }

    public int EmployeeCount { get; set; }

    public decimal TotalWorkingHours { get; set; }

    public decimal TotalLeaveHours { get; set; }

    public decimal TotalActualWorkingHours { get; set; }

    public decimal TotalBaseSalary { get; set; }

    public decimal TotalNetPay { get; set; }

    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = null!;
}

public class PayrollListResponseModel : PagedResult<PayrollResponseModel>
{
}

public class PayrollMonthDetailResponseModel
{
    public string PayrollId { get; set; } = null!;

    public string PayrollCode { get; set; } = null!;

    public string? EmployeeCode { get; set; }
    public string? EmployeeName { get; set; }

    public DateTime? PayrollDate { get; set; }

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
}

public class PayrollMonthDetailListResponseModel : PagedResult<PayrollMonthDetailResponseModel>
{
}

public class EmployeePayrollResponseModel
{
    public string PayrollId { get; set; } = null!;

    public string PayrollCode { get; set; } = null!;

    public string? EmployeeCode { get; set; }

    public DateTime? PayrollDate { get; set; }

    public string PayrollMonth { get; set; }
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
}

public class EmployeePayrollListResponseModel : PagedResult<EmployeePayrollResponseModel>
{
}