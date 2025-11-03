using HRSystem.Csharp.Domain.Features.MenuGroup;
using HRSystem.Csharp.Domain.Models.Common;
using HRSystem.Csharp.Domain.Models.MenuGroup;
using HRSystem.Csharp.Shared;

namespace HRSystem.Csharp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MenuGroupController : ControllerBase
{
    private readonly BL_MenuGroup _blMenuGroup;

    public MenuGroupController(BL_MenuGroup blMenuGroup)
    {
        _blMenuGroup = blMenuGroup;
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetAll([FromQuery]PaginationRequestModel model)
    {
        var response = await _blMenuGroup.GetAllMenuGroups(model);
        if (response.IsSuccess)
        {
            return Ok(response);
        }

        return BadRequest(response);
    }

    [HttpGet("edit/{menuGroupCode}")]
    public async Task<IActionResult> Get(string menuGroupCode)
    {
        if (string.IsNullOrWhiteSpace(menuGroupCode))
        {
            var menuGpCode = Result<bool>.ValidationError("Menu code is required!");
            return BadRequest(menuGpCode);
        }

        var response = await _blMenuGroup.GetMenuGroup(menuGroupCode);
        if (response.IsSuccess)
        {
            return Ok(response);
        }

        return BadRequest(response);
    }

    [HttpPut("update/{menuGroupCode}")]
    public async Task<IActionResult> Put(string menuGroupCode, [FromBody] MenuGroupUpdateRequestModel menuGroup)
    {
        if (string.IsNullOrWhiteSpace(menuGroupCode))
        {
            var menuGpCode = Result<bool>.ValidationError("Menu code is required!");
            return BadRequest(menuGpCode);
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var response = await _blMenuGroup.UpdateMenuGroup(menuGroupCode, menuGroup);
        if (response.IsSuccess)
        {
            return Ok(response);
        }

        return BadRequest(response);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateMenuGroupAsync(MenuGroupRequestModel requestMenuGroup)
    {
        if (requestMenuGroup is null)
        {
            var menuGpCode = Result<bool>.BadRequestError("Invalid request data!");
            return BadRequest(menuGpCode);
        }

        var result = await _blMenuGroup.CreateMenuGroupAsync(requestMenuGroup);
        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }


    [HttpDelete("delete/{menuGroupCode}")]
    public async Task<IActionResult> DeleteMenuGroup(string menuGroupCode)
    {
        if (string.IsNullOrWhiteSpace(menuGroupCode))
        {
            var menuGpCode = Result<bool>.ValidationError("Menu code is required!");
            return BadRequest(menuGpCode);
        }

        var result = await _blMenuGroup.DeleteMenuGroupAsync(menuGroupCode);
        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
}