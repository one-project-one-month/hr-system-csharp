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
}
