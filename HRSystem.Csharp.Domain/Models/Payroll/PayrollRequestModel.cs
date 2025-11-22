using HRSystem.Csharp.Domain.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Csharp.Domain.Models.Payroll
{
    public class PayrollRequestModel : PaginationRequestModel
    {
        public string? EmployeeName { get; set; }
        public string? EmployeeCode { get; set; }
    }
}
