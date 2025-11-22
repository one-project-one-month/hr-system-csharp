using HRSystem.Csharp.Domain.Features.Attendance;
using HRSystem.Csharp.Domain.Models.Attendance;
using System.Security.Claims;

namespace HRSystem.Csharp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AttendanceController : ControllerBase
{
    private readonly BL_Attendance _bL_Attendance;

    public AttendanceController(BL_Attendance bL_Attendance)
    {
        _bL_Attendance = bL_Attendance;
    }

    [HttpGet("AttendanceList")]
    public async Task<IActionResult> AttendanceLists(String? EmpName, DateTime startDate, DateTime endDate, int pageNo=1, int PageSize=10)
    {
        var data = await _bL_Attendance.List(EmpName,startDate, endDate, pageNo, PageSize);
        return Ok(data);
    }

    [HttpPost("AttendanceCreate")]
    public async Task<IActionResult> AttendanceCreate(AttendanceCreateRequestModel requestModel)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
            return Unauthorized("Invalid user token.");

        var data = await _bL_Attendance.Create(userId,requestModel);
        return Ok(data);
    }

    [HttpPut("AttendanceUpdate")]
    public async Task<IActionResult> AttendanceUpdate(string userId, AttendanceUpdateRequestModel requestModel)
    {
        var data = await _bL_Attendance.Update(userId, requestModel);
        return Ok(data);
    }

    [HttpGet("edit/{attendanceCode}")]
    public async Task<IActionResult> AttendanceEdit(string attendanceCode)
    {
        var data = await _bL_Attendance.Edit(attendanceCode);
        return Ok(data);
    }

    [HttpDelete("delete/{attendanceCode}")]
    public async Task<IActionResult> AttendanceDelete(string attendanceCode)
    {
        var data = await _bL_Attendance.Delete(attendanceCode);
        return Ok(data);
    }
}