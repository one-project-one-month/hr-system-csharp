using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Csharp.Domain.Models.Reports
{
    public class StaffAttendanceOverviewReport
    {
        public List<StaffAttendanceOverviewReportModel> staffAttendanceOverview { get; set; } = new List<StaffAttendanceOverviewReportModel>();

    }

    public class StaffAttendanceOverviewReportModel
    {
        public string month { get; set; } = string.Empty;
        public int present { get; set; } = 0;
        public int late { get; set; } = 0;
    }
}
