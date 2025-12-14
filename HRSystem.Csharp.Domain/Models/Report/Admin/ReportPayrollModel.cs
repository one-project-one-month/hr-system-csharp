namespace HRSystem.Csharp.Domain.Models.Report.Admin;

public class ReportPayrollModel
{
    public string PayrollCode { get; set; } = null!;
    public string EmployeeName { get; set; } = null!;
    public string PayrollMonth { get; set; } = null!;
    public string TotalWorkingHour { get; set; } = null!;
    public string LeaveHour { get; set; } = null!;
    public string ActualWorkingHour { get; set; } = null!;
    public string BaseSalary { get; set; } = null!;
    public string GrossPay { get; set; } = null!;
    public string Deduction { get; set; } = null!;
    public string NetPay { get; set; } = null!;
}