using HRSystem.Csharp.Domain.Features;
using HRSystem.Csharp.Domain.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HRSystem.Csharp.Api.Controllers
{
    [Route("api/")]
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

        [HttpGet("menu/{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _blMenu.GetMenu(id);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest(result);
        }

        [HttpPost("menu")]
        public async Task<IActionResult> Post([FromBody] MenuCreateModel menu)
        {
            var result = await _blMenu.CreateMenu(menu);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest(result);
        }

        [HttpPut("menu/{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] MenuCreateModel menu)
        {
            var result = await _blMenu.UpdateMenu(id, menu);
            return result.IsSuccess ? Ok(result.Data) : BadRequest(result);
        }

        [HttpDelete("menu/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _blMenu.DeleteMenu(id);
            return result.IsSuccess ? Ok(result.Data) : BadRequest(result);
        }
    }
}
