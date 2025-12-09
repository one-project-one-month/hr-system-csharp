using HRSystem.Csharp.Domain.Models.Common;

namespace HRSystem.Csharp.Domain.Models.Payroll;

public class PayrollRequestModel : PaginationRequestModel
{
    public string? EmployeeName { get; set; }
    public string? EmployeeCode { get; set; }
}

public class PayrollProcessRequestModel
{
    public string PayrollMonth { get; set; } = string.Empty;

    // public string CreatedBy { get; set; } = string.Empty;
}

public class PayrollListRequestModel : PaginationRequestModel
{
    public string? MonthYear { get; set; }
}

public class PayrollMonthDetailListRequestModel : PaginationRequestModel
{
    public string PayrollSummaryCode { get; set; } = string.Empty;
    public string? EmployeeName { get; set; }
}

public class EmployeePayrollListRequestModel : PaginationRequestModel
{
    public string? MonthYear { get; set; }
}

public class MonthlyPayrollChartRequestModel
{
    public string Year { get; set; }
}