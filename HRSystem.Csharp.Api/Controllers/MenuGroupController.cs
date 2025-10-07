using HRSystem.Csharp.Domain.Features;
using HRSystem.Csharp.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HRSystem.Csharp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuGroupController : ControllerBase
    {
        private readonly BL_MenuGroup _blMenuGroup;
        public MenuGroupController(BL_MenuGroup blMenuGroup)
        {
            _blMenuGroup = blMenuGroup;
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
