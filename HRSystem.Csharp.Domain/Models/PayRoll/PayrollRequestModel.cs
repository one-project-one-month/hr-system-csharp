using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Csharp.Domain.Models.PayRoll
{
    public class PayrollRequestModel
    {

    }
    public class PayrollCreateRequestModel
    {
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
    }

    public class PayrollUpdateRequestModel
    {
        public string? PayrollCode { get; set; }
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
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }

        public string? EmployeeCode { get; set; }
        public DateTime? PayrollDate { get; set; }
        
    }
}