using HRSystem.Csharp.Domain.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Csharp.Domain.Models.Payroll
{
    public class PayrollResponseModel
    {
        public string PayrollId { get; set; } = null!;
        public string EmployeeCode { get; set; } = null!;
        public DateOnly PayrollDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public string Status { get; set; } = null!;
        public int? TotalWorkingHour { get; set; } = 0;
        public decimal? LeaveHour { get; set; } = 0;
        public decimal? GrossPay { get; set; } = 0;
        public decimal? NetPay { get; set; } = 0;
        public decimal? BaseSalary { get; set; } = 0;
        public string? EmployeeName { get; set; } = null!;
        public decimal? Deduction { get; set; }
    }

    public class PayrollListResponseModel : PagedResult<PayrollResponseModel>
    {

    }
}
