using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Csharp.Domain.Models.Leave;

    public class LeaveResponseModel
    {
        public string LeaveId { get; set; } = null!;

        public string EmployeeCode { get; set; } = null!;

    public string EmployeeName { get; set; } = null!;

        public string LeaveType { get; set; } = null!;

        public string Reason { get; set; } = null!;

        public DateOnly FromDate { get; set; }

        public DateOnly ToDate { get; set; }

        public decimal? TotalHours { get; set; }

        public bool? IsPaid { get; set; }

        public string Status { get; set; } = null!;

        public string FullOrHalf { get; set; } = null!;

        public string LeaveCode { get; set; } = null!;
    }

    public class LeaveListResponseModel : PagedResult<LeaveResponseModel>
    {
    }

