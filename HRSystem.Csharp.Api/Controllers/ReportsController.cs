using HRSystem.Csharp.Domain.Features.Reports;
using Microsoft.AspNetCore.Mvc;

namespace HRSystem.Csharp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : Controller
    {
        private readonly BL_AttendanceReports _bL_AttendanceReports;

        public ReportsController(BL_AttendanceReports bL_AttendanceReports)
        {
            _bL_AttendanceReports = bL_AttendanceReports;
        }

        [HttpGet("GetHRAttendanceReport")]
        public async Task<IActionResult> AttendanceReport(String date, int dataView) // dataView = 0 for currentDay, 1 for weekly, 2 for monthly, 3 for yearly
        {
            var data = await _bL_AttendanceReports.GetHRAttendanceOverviewReport(date, dataView);
            return Ok(data);
        }

        [HttpGet("GetStaffAttendanceReport")]
        public async Task<IActionResult> StaffAttendanceReport(int year, String empCode)
        {
            var data = await _bL_AttendanceReports.GetStaffAttendanceOverviewReport(year, empCode);
            return Ok(data);

        }
    }
}
