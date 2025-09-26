using HRSystem.Csharp.Domain.DTOs;
using HRSystem.Csharp.Domain.Features.Roles;
using HRSystem.Csharp.Domain.Models.Roles;
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

        [HttpPost("CreateNewRole")]
        public IActionResult CreateNewRole([FromBody] RoleRequestModel role)
        {
            if (role == null)
            {
                return BadRequest("New Role Cannot Be Null!");
            }
            var result = _blRole.CreateRole(role);
            if (result.IsSuccess) {
                return Ok(result.Data);
            }
            return BadRequest(result);

        }
        [HttpGet("GetRole/{roleCode}")]
        public IActionResult GetRole(string roleCode)
        {
            if (string.IsNullOrEmpty(roleCode)){
                return BadRequest("Role code cannot be empty");
            }
            var result = _blRole.EditRole(roleCode);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest(result);
        }

        [HttpPatch("UpdateRole/{roleCode}")]
        public IActionResult UpdateRole([FromBody] RoleUpdateRequestModel role,string roleCode)
        {
            if (string.IsNullOrEmpty(roleCode))
            {
                return BadRequest("Role code cannot be empty");
            }
            if (role == null) {
                return BadRequest("Role to be updated cannot be empty");
            }
            var result = _blRole.UpdateRole(role, roleCode);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest(result);
        }

        [HttpDelete("{roleCode}")]
        public IActionResult DeleteRole(string roleCode)
        {
            if (string.IsNullOrEmpty(roleCode))
            {
                return BadRequest("Role code cannot be empty");
            }
            var result = _blRole.DeleteRole(roleCode);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest(result);
        }

    }
}