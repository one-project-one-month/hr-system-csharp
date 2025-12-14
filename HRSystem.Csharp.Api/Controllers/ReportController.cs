using HRSystem.Csharp.Domain.Features.Reports;

namespace HRSystem.Csharp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReportController : ControllerBase
{
    private readonly BL_ReportService _blReportService;

    public ReportController(BL_ReportService blReportService)
    {
        _blReportService = blReportService;
    }

    #region Attendance

    /*[HttpPost("attendance")]
    public async Task<IActionResult> AttendanceReport()
    {
        
    }*/

    #endregion

    #region Payroll

    

    #endregion

    #region Backlog

    

    #endregion
    
}