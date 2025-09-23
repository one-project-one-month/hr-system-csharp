using HRSystem.Csharp.Domain.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRSystem.Csharp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly BL_Role _blRole;
        public RoleController(BL_Role blRole)
        {
            _blRole = blRole;
        }
        [HttpGet("GetAllRoles")]
        public IActionResult GetAllRoles()
        {
            var result = _blRole.GetAllRoles();
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest(result);
        }
    }
}