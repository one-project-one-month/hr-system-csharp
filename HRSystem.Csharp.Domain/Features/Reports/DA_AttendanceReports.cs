using Dapper;
using HRSystem.Csharp.Domain.Models.Reports;
using HRSystem.Csharp.Shared.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Csharp.Domain.Features.Reports
{
    public class DA_AttendanceReports
    {
        private readonly DapperService _dapperService;
        public DA_AttendanceReports(DapperService dapperService)
        {
            _dapperService = dapperService;
        }
        public async Task<HRAttendanceOverviewReport> GetHRAttendanceOverviewReporttAsync(String Date, int dataView)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Date", Date, DbType.Date, ParameterDirection.Input);
            parameters.Add("@DataView", dataView, DbType.Int32, ParameterDirection.Input);

            var results = await _dapperService.QueryStoredProcedureWithMultipleResults<HRAttendanceOverviewReport>("sp_HRAttendanceDashboard", parameters);

            HRAttendanceOverviewReport overviewReport = new HRAttendanceOverviewReport();
            
            var first = results.FirstOrDefault();

            overviewReport.Present = first?.Present ?? 0;
            overviewReport.Absent = first?.Absent ?? 0;
            overviewReport.Late = first?.Late ?? 0;
            overviewReport.EmpCount = first?.EmpCount ?? 0;

            return overviewReport;
        }

        public async Task<StaffAttendanceOverviewReport> GetStaffAttendanceOverviewReporttAsync(int Year, String empCode)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Year", Year, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@EmpCode", empCode, DbType.String, ParameterDirection.Input);

            var results = await _dapperService.QueryStoredProcedureWithMultipleResults<StaffAttendanceOverviewReportModel>("sp_EmpAttendanceDashboard", parameters);

            var overviewReport = new StaffAttendanceOverviewReport();

            foreach (var item in results)
            {
                var reportModel = new StaffAttendanceOverviewReportModel
                {
                    month = item.month,
                    present = item.present,
                    late = item.late
                };
                overviewReport.staffAttendanceOverview.Add(reportModel);
            }

            return overviewReport;
        }
    }
}
