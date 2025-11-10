using HRSystem.Csharp.Database.AppDbContextModels;
using HRSystem.Csharp.Domain.Features.Menu;
using HRSystem.Csharp.Domain.Models.Menu;
using System.Security.Claims;
using HRSystem.Csharp.Shared;
using HRSystem.Csharp.Domain.Models.Common;

namespace HRSystem.Csharp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MenuController : ControllerBase
{
    private readonly BL_Menu _blMenu;

    public MenuController(BL_Menu blMenu)
    {
        _blMenu = blMenu;
    }

    [HttpGet("list")]
    public async Task<IActionResult> Get([FromQuery] PaginationRequestModel model)
    {
        var result = await _blMenu.GetAllMenus(model);
        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateMenu([FromBody] MenuRequestModel requestMenu)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
            return Unauthorized("Invalid user token.");

        var result = await _blMenu.CreateMenuAsync(userId, requestMenu);
        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("edit/{menuCode}")]
    public async Task<IActionResult> Get(string menuCode)
    {
        if (string.IsNullOrWhiteSpace(menuCode))
        {
            var response = Result<bool>.ValidationError("Menu code is required!");
            return BadRequest(response);
        }

        var result = await _blMenu.GetMenu(menuCode);
        if (result.IsSuccess)
            return Ok(result);

        return BadRequest(result);
    }

    [HttpPut("update/{menuCode}")]
    public async Task<IActionResult> Put(string menuCode, [FromBody] MenuUpdateRequestModel menu)
    {
        if (string.IsNullOrWhiteSpace(menuCode))
        {
            var response = Result<bool>.ValidationError("Menu code is required!");
            return BadRequest(response);
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
            return Unauthorized("Invalid user token.");

        var updatingMenu = new TblMenu
        {
            MenuCode = menuCode,
            MenuName = menu.MenuName,
            MenuGroupCode = menu.MenuGroupCode,
            Url = menu.Url,
            Icon = menu.Icon,
            SortOrder = menu.SortOrder,
        };

        var result = await _blMenu.UpdateMenu(userId, updatingMenu);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("delete/{menuCode}")]
    public async Task<IActionResult> DeleteMenu(string menuCode)
    {
        if (string.IsNullOrWhiteSpace(menuCode))
        {
            var response = Result<bool>.ValidationError("Menu code is required!");
            return BadRequest(response);
        }

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
            return Unauthorized("Invalid user token.");

        var result = await _blMenu.DeleteMenuAsync(userId, menuCode);
        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
}