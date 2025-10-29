using HRSystem.Csharp.Domain.Features.Roles;
using HRSystem.Csharp.Domain.Models.Roles;
using System.Threading.Tasks;

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
    public async Task<IActionResult> CreateRole([FromBody] RoleRequestModel role)
    {
        var result = await _blRole.CreateRole(role);
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }
        return BadRequest(result);
    }

    [HttpGet("GetAllRoles")]
    public async Task<IActionResult> GetAllRoles()
    {
        var result = await _blRole.GetAllRoles();
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }
        return BadRequest(result);
    }

    [HttpGet("GetRole/{roleCode}")]
    public async Task<IActionResult> GetRoleByCode(string roleCode)
    {
        var result = await _blRole.GetRoleByCode(roleCode);
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }
        return BadRequest(result);
    }

    [HttpPatch("UpdateRole/{roleCode}")]
    public async Task<IActionResult> UpdateRole(string roleCode, [FromBody] RoleUpdateRequestModel role)
    {
        var result = await _blRole.UpdateRole(role, roleCode);
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }
        return BadRequest(result);
    }

    [HttpDelete("DeleteRole/{roleCode}")]
    public async Task<IActionResult> DeleteRole(string roleCode)
    {
        var result = await _blRole.DeleteRole(roleCode);
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }
        return BadRequest(result);
    }
}