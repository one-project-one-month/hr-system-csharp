namespace HRSystem.Csharp.Domain.Models.Payroll;

public class PayrollResponseModel
{
    public string PayrollId { get; set; }
    public string PayrollCode { get; set; }
    public string EmployeeCode { get; set; }
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
    public string EmployeeName { get; set; }
}

public class PayrollListResponseModel : PagedResult<PayrollResponseModel>
{
}