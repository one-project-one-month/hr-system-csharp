namespace HRSystem.Csharp.Api.Controllers;

[Route("api")]
[ApiController]
public class EmployeeAttendanceController : ControllerBase
{
    private readonly BL_EmployeeAttendance _bl_EmployeeAttendance;

    public EmployeeAttendanceController(BL_EmployeeAttendance bl_EmployeeAttendance)
    {
        _bl_EmployeeAttendance = bl_EmployeeAttendance;
    }

    [HttpPost("employee/check-in-out")]
    public async Task<IActionResult> AttendanceCheck(EmployeeAttendanceRequestModel requestModel)
    {
        var response = await _bl_EmployeeAttendance.AttendanceCheck(requestModel);
        if (!response.IsSuccess)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpGet("employee/today-attendance/{employeeCode}")]
    public async Task<IActionResult> GetAttendanceForToday(string employeeCode)
    {
        if (employeeCode == null) return BadRequest("Employee Code is required!");

        var response = await _bl_EmployeeAttendance.GetAttendanceForToday(employeeCode);
        if (response.IsSuccess) return Ok(response);

        return BadRequest(response);

    }
}