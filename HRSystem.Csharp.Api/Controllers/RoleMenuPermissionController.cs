using HRSystem.Csharp.Domain.Features.RoleMenuPermission;
using HRSystem.Csharp.Domain.Models.RoleMenuPermission;

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
    public async Task<IActionResult> GetMenuTreeWithPermissions([FromQuery] MenuTreeRequestModel reqModel)
    {
        try
        {
            var result = await _blRoleMenuPermission.GetMenuTreeWithPermissionsAsync(reqModel);
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

    [HttpPost("permissions/list")]
    public async Task<IActionResult> GetPermissions()
    {
        try
        {
            var result = await _blRoleMenuPermission.GetAllPermissions();
            return Ok(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
            return StatusCode(500, "Unexpeced error occurred");
        }
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateRoleMenuPermission(CreateRoleMenuPermissionRequestModel reqModel)
    {
        try
        {
            var result = await _blRoleMenuPermission.CreateRoleMenuPermission(reqModel);
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