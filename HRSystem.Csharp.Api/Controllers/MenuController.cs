using HRSystem.Csharp.Domain.Features.Menu;
using HRSystem.Csharp.Domain.Models.Menu;

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

    [HttpGet("menus")]
    public async Task<IActionResult> Get()
    {
        var result = await _blMenu.GetAllMenus();
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }
        return BadRequest(result);
    }

    [HttpPost("menu")]
    public async Task<IActionResult> CreateMenu([FromBody] MenuRequestModel requestMenu)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _blMenu.CreateMenuAsync(requestMenu);
        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("menu/{menuCode}")]
    public async Task<IActionResult> Get(string menuCode)
    {
        var result = await _blMenu.GetMenu(menuCode);
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }

        return BadRequest(result);
    }

    [HttpPut("menu/{menuCode}")]
    public async Task<IActionResult> Put(string menuCode, [FromBody] MenuRequestModel menu)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _blMenu.UpdateMenu(menuCode, menu);
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result);
    }

    [HttpDelete("menu/{menuCode}")]
    public async Task<IActionResult> DeleteMenu(string menuCode)
    {
        var result = await _blMenu.DeleteMenuAsync(menuCode);
        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
}