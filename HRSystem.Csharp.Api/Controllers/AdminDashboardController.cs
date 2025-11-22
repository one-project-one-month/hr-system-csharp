using HRSystem.Csharp.Domain.Features.AdminDashboard;
using Microsoft.AspNetCore.Mvc;

namespace HRSystem.Csharp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AdminDashboardController : Controller
{
        private readonly BL_AdminDashboard _blAdminDashboard;

        public AdminDashboardController(BL_AdminDashboard blAdminDashboard)
        {
                _blAdminDashboard = blAdminDashboard;
        }

        [HttpGet("stats-cards")]
        public async Task<IActionResult> GetStatsCards()
        {
                var result = await _blAdminDashboard.GetStatsCards();
                if(result.IsSuccess) return Ok(result);

                return StatusCode(500, result);
        }

        [HttpGet("attendances-histogram/{type}")]
        public async Task<IActionResult> GetAttendanceHistogram(string type)
        {
                var result = await _blAdminDashboard.GetAttendanceHistogram(type);

                if(result.IsSuccess) return Ok( result);

                if(result.IsNotFound) return NotFound(result);

                if(result.IsInvalidData) return BadRequest(result);

                return StatusCode(500, result);
        }
}
