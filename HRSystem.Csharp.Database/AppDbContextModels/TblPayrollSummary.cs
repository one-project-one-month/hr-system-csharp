using System;
using System.Collections.Generic;

namespace HRSystem.Csharp.Database.AppDbContextModels;

public partial class TblPayrollSummary
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

    public string PayrollSummaryCode { get; set; } = null!;
}
