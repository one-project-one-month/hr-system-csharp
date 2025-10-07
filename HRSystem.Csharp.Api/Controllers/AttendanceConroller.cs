using HRSystem.Csharp.Domain.Features.Attendance;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRSystem.Csharp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceConroller : ControllerBase
    {
        private readonly BL_Attendance _bL_Attendance;

        public AttendanceConroller(BL_Attendance bL_Attendance)
        {
            _bL_Attendance = bL_Attendance;
        }

        [HttpGet("AttendanceList")]
        public async Task<IActionResult> AttendanceLists()
        {
            var data = await _bL_Attendance.List();
            return Ok(data);
        }
    }
}
