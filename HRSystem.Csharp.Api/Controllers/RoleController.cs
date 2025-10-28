using HRSystem.Csharp.Domain.Features.Roles;
using HRSystem.Csharp.Domain.Models.Roles;

namespace HRSystem.Csharp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoleController : ControllerBase
{
    private readonly BL_Role _blRole;

    public RoleController(BL_Role blRole)
    {
        _blRole = blRole;
    }

    [HttpPost("CreateRole")]
    public IActionResult CreateRole([FromBody] RoleRequestModel role)
    {
        var result = _blRole.CreateRole(role);
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }
        return BadRequest(result);
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

    [HttpGet("GetRole/{roleCode}")]
    public IActionResult GetRoleByCode(string roleCode)
    {
        var result = _blRole.GetRoleByCode(roleCode);
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }
        return BadRequest(result);
    }

    [HttpPatch("UpdateRole/{roleCode}")]
    public IActionResult UpdateRole(string roleCode, [FromBody] RoleUpdateRequestModel role)
    {
        var result = _blRole.UpdateRole(role, roleCode);
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }
        return BadRequest(result);
    }

    [HttpDelete("DeleteRole/{roleCode}")]
    public IActionResult DeleteRole(string roleCode)
    {
        var result = _blRole.DeleteRole(roleCode);
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }
        return BadRequest(result);
    }
}