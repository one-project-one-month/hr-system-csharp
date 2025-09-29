using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Csharp.Domain.Models
{
    public class AttendanceListResponseModel
    {
        public Guid AttendanceId { get; set; }

        public string? AttendanceCode { get; set; }

        public string? EmployeeCode { get; set; }

        public DateTime? AttendanceDate { get; set; }

        public DateTime? CheckInTime { get; set; }

        public string? CheckInLocation { get; set; }

        public DateTime? CheckOutTime { get; set; }

        public string? CheckOutLocation { get; set; }

        public decimal? WorkingHour { get; set; }

        public string? Status { get; set; }

        public bool? IsSavedLocation { get; set; }

    }
}
