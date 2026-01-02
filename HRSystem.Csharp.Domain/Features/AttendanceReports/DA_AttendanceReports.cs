using Dapper;
using HRSystem.Csharp.Domain.Models.AttendanceReports;
using HRSystem.Csharp.Shared.Services;
using static HRSystem.Csharp.Domain.Models.AttendanceReports.AdminAttendanceOverviewReport;

namespace HRSystem.Csharp.Domain.Features.AttendanceReports;

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

        var results =
            await _dapperService.QueryStoredProcedureWithMultipleResults<HRAttendanceOverviewReport>(
                "sp_HRAttendanceDashboard", parameters);

        HRAttendanceOverviewReport overviewReport = new HRAttendanceOverviewReport();

        var first = results.FirstOrDefault();

        overviewReport.Present = first?.Present ?? 0;
        overviewReport.Absent = first?.Absent ?? 0;
        overviewReport.Late = first?.Late ?? 0;
        overviewReport.EmpCount = first?.EmpCount ?? 0;

        return overviewReport;
    }

    public async Task<StaffAttendanceOverviewReport> GetStaffAttendanceOverviewReportAsync(int Year, String empCode)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Year", Year, DbType.Int32, ParameterDirection.Input);
        parameters.Add("@EmpCode", empCode, DbType.String, ParameterDirection.Input);

        var results =
            await _dapperService.QueryStoredProcedureWithMultipleResults<StaffAttendanceOverviewReportModel>(
                "sp_EmpAttendanceDashboard", parameters);

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

    public async Task<AdminAttendanceOverviewReport> GetAdminAttendanceOverviewReportAsync(String Date, int dataView)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Date", Date, DbType.Date, ParameterDirection.Input);
        parameters.Add("@DataView", dataView, DbType.Int32, ParameterDirection.Input);

        var results =
            await _dapperService.QueryStoredProcedureWithMultipleResults<AdminAttendanceOverviewReportModel>(
                "sp_AdminAttendanceDashboard", parameters);

        var overviewReport = new AdminAttendanceOverviewReport();

        foreach (var item in results)
        {
            var reportModel = new AdminAttendanceOverviewReportModel
            {
                Date = item.Date,
                Present = item.Present,
                Absent = item.Absent,
                HalfDayLeave = item.HalfDayLeave,
                EmpCount = item.EmpCount,
                ProjCount = item.ProjCount,
                TdyAbsent = item.TdyAbsent
            };
            overviewReport.adminAttendanceOverviewReports.Add(reportModel);
        }
        return overviewReport;
    }
}