using HRSystem.Csharp.Domain.Features;
using HRSystem.Csharp.Domain.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HRSystem.Csharp.Api.Controllers
{
    [Route("api/")]
    [ApiController]
    public class MenuGroupController : ControllerBase
    {
        private readonly BL_MenuGroup _blMenuGroup;
        public MenuGroupController(BL_MenuGroup blMenuGroup)
        {
            _blMenuGroup = blMenuGroup;
        }
        [HttpGet("menu-groups")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _blMenuGroup.GetAllMenuGroups();
            if(response.IsSuccess)
            {
                return Ok(response.Data);
            }
            return BadRequest(response);
        }

        [HttpGet("menu-group/{menuGroupCode}")]
        public async Task<IActionResult> Get(string menuGroupCode)
        {
             var response = await _blMenuGroup.GetMenuGroup(menuGroupCode);
            if (response.IsSuccess)
            {
                return Ok(response.Data);
            }
            return BadRequest(response);
        }

        [HttpPut("menu-group/{menuGroupCode}")]
        public async Task<IActionResult> Put(string menuGroupCode, [FromBody] MenuGroupUpdateRequestModel menuGroup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _blMenuGroup.UpdateMenuGroup(menuGroupCode, menuGroup);
            if(response.IsSuccess)
            {
                return Ok(response.Data);
            }   
            return BadRequest(response);
        }
        
        [HttpPost("menu-group")]
        public async Task<IActionResult> CreateMenuGroupAsync(MenuGroupRequestModel requestMenuGroup)
        {
            var result = await _blMenuGroup.CreateMenuGroupAsync(requestMenuGroup);
            if(result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }


        [HttpDelete("menu-group/{menuGroupCode}")]
        public async Task<IActionResult> DeleteMenuGroup(string menuGroupCode)
        {
            var result = await _blMenuGroup.DeleteMenuGroupAsync(menuGroupCode);
            if(result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
