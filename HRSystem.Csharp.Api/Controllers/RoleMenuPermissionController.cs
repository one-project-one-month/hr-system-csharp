using HRSystem.Csharp.Domain.Features.RoleMenuPermission;

namespace HRSystem.Csharp.Api.Controllers;

[Route("api/role-menu-permission")]
[ApiController]
public class RoleMenuPermissionController : ControllerBase
{
    private readonly ILogger<RoleMenuPermissionController> _logger;
    private readonly BL_RoleMenuPermission _blRoleMenuPermission;

    public RoleMenuPermissionController(ILogger<RoleMenuPermissionController> logger,
        BL_RoleMenuPermission blRoleMenuPermission)
    {
        _logger = logger;
        _blRoleMenuPermission = blRoleMenuPermission;
    }

    [HttpGet("menu-tree")]
    public async Task<IActionResult> GetMenuTree()
    {
        try
        {
            var result = await _blRoleMenuPermission.GetMenuTreeAsync();
            if (result.IsError)
                return BadRequest(result);

            return Ok(result.Data);
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
            return StatusCode(500, "Unexpected error occurred.");
        }
    }
}