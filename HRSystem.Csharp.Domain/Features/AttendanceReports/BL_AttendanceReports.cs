using HRSystem.Csharp.Domain.Models.AttendanceReports;

namespace HRSystem.Csharp.Domain.Features.AttendanceReports;

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
        var result = await _daAttendanceReports.GetStaffAttendanceOverviewReportAsync(Year, empCode);
        return result;
    }

    public async Task<AdminAttendanceOverviewReport> GetAdminAttendanceOverviewReport(String Date, int dataView)
    {
        var result = await _daAttendanceReports.GetAdminAttendanceOverviewReportAsync(Date, dataView);
        return result;
    }
}