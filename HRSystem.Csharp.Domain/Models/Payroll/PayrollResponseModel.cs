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