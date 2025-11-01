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

    [HttpPost("create")]
    public async Task<IActionResult> CreateRole([FromBody] RoleRequestModel role)
    {
        var result = await _blRole.CreateRole(role);
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }

        return BadRequest(result);
    }

    [HttpPost("list")]
    public async Task<IActionResult> GetAllRoles([FromBody] RoleListRequestModel reqModel)
    {
        var result = await _blRole.GetAllRoles(reqModel);
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }

        return BadRequest(result);
    }

    [HttpPost("edit")]
    public async Task<IActionResult> GetRoleByCode(RoleEditRequestModel reqModel)
    {
        var result = await _blRole.GetRoleByCode(reqModel);
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }

        return BadRequest(result);
    }

    [HttpPatch("update")]
    public async Task<IActionResult> UpdateRole([FromBody] RoleUpdateRequestModel reqModel)
    {
        var result = await _blRole.UpdateRole(reqModel);
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }

        return BadRequest(result);
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteRole([FromBody] RoleDeleteRequestModel reqModel)
    {
        var result = await _blRole.DeleteRole(reqModel);
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }

        return BadRequest(result);
    }
}