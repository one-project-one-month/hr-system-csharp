using HRSystem.Csharp.Domain.Features.Attendance;
using HRSystem.Csharp.Domain.Models.Attendance;

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
    public async Task<IActionResult> AttendanceLists(int pageNo=1, int PageSize=10)
    {
        var data = await _bL_Attendance.List(pageNo, PageSize);
        return Ok(data);
    }

    [HttpPost("AttendanceCreate")]
    public async Task<IActionResult> AttendanceCreate(AttendanceCreateRequestModel requestModel)
    {
        var data = await _bL_Attendance.Create(requestModel);
        return Ok(data);
    }

    [HttpPut("AttendanceUpdate")]
    public async Task<IActionResult> AttendanceUpdate(AttendanceUpdateRequestModel requestModel)
    {
        var data = await _bL_Attendance.Update(requestModel);
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