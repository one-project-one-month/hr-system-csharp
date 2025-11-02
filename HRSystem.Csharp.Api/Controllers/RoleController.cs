using HRSystem.Csharp.Domain.Models.Roles;
using System.Threading.Tasks;
using HRSystem.Csharp.Domain.Features.Role;
using HRSystem.Csharp.Shared;

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

    [HttpGet("list")]
    public async Task<IActionResult> GetAllRoles([FromQuery] RoleListRequestModel reqModel)
    {
        var result = await _blRole.GetAllRoles(reqModel);
        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateRole([FromBody] RoleRequestModel role)
    {
        if (role == null)
        {
            var response = Result<bool>.ValidationError("Role data is required!");
            return BadRequest(response);
        }

        if (string.IsNullOrWhiteSpace(role.RoleName))
        {
            var response = Result<bool>.ValidationError("Role name is required!");
            return BadRequest(response);
        }

        var result = await _blRole.CreateRole(role);
        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("edit/{roleCode}")]
    public async Task<IActionResult> GetRoleByCode(string roleCode)
    {
        if (string.IsNullOrWhiteSpace(roleCode))
        {
            var response = Result<bool>.ValidationError("Role code is required!");
            return BadRequest(response);
        }

        var result = await _blRole.GetRoleByCode(new RoleEditRequestModel()
        {
            RoleCode = roleCode
        });

        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPut("update/{roleCode}")]
    public async Task<IActionResult> UpdateRole(string roleCode, [FromBody] RoleUpdateRequestModel reqModel)
    {
        if (string.IsNullOrWhiteSpace(roleCode))
        {
            var response = Result<bool>.ValidationError("Role name is required!");
            return BadRequest(response);
        }

        var result = await _blRole.UpdateRole(roleCode, reqModel);
        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpDelete("delete/{roleCode}")]
    public async Task<IActionResult> DeleteRole(string roleCode)
    {
        if (string.IsNullOrWhiteSpace(roleCode))
        {
            var response = Result<bool>.ValidationError("Role code is required!");
            return BadRequest(response);
        }

        var result = await _blRole.DeleteRole(new RoleDeleteRequestModel()
        {
            RoleCode = roleCode
        });

        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
}