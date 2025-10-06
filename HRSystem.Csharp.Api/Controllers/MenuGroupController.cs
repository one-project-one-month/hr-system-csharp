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
        [HttpGet("MenuGroups")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _blMenuGroup.GetAllMenuGroups();
            if(response.IsSuccess)
            {
                return Ok(response.Data);
            }
            return BadRequest(response);
        }

        [HttpGet("MenuGroup/{id}")]
        public async Task<IActionResult> Get(string id)
        {
             var response = await _blMenuGroup.GetMenuGroup(id);
            if (response.IsSuccess)
            {
                return Ok(response.Data);
            }
            return BadRequest(response);
        }


        [HttpPut("MenuGroup/{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] MenuGroupRequestModel menuGroup)
        {
            var response = await _blMenuGroup.UpdateMenuGroup(id, menuGroup);
            if(response.IsSuccess)
            {
                return Ok(response.Data);
            }   
            return BadRequest(response);
        }
        
        [HttpPost("menugroup")]
        public async Task<IActionResult> CreateMenuGroupAsync(MenuGroup requestMenuGroup)
        {
            var result = await _blMenuGroup.CreateMenuGroupAsync(requestMenuGroup);
            if(result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }


        [HttpDelete("menugroup/{menuGroupCode}")]
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
