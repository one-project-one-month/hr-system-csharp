using HRSystem.Csharp.Domain.Models.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Csharp.Domain.Features.Reports
{
    public class BL_AttendanceReports
    {
        private readonly DA_AttendanceReports _daAttendanceReports;
        public BL_AttendanceReports(DA_AttendanceReports daAttendanceReports)
        {
            _daAttendanceReports = daAttendanceReports;
        }

        public async Task<HRAttendanceOverviewReport> GetHRAttendanceOverviewReport(String Date, int dataView)
        {
            var result = await _daAttendanceReports.GetHRAttendanceOverviewReporttAsync(Date, dataView);
            return result;
        }

        public async Task<StaffAttendanceOverviewReport> GetStaffAttendanceOverviewReport(int Year, String empCode)
        {
            var result = await _daAttendanceReports.GetStaffAttendanceOverviewReporttAsync(Year, empCode);
            return result;
        }
    }
}
