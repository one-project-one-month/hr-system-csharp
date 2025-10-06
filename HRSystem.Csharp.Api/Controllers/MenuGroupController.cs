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


        [HttpPost("create-menugroup")]
        public async Task<IActionResult> CreateMenuGroupAsync(MenuGroup requestMenuGroup)
        {
            var result = await _blMenuGroup.CreateMenuGroupAsync(requestMenuGroup);
            if(result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }


        [HttpDelete("delete-menugroup/{menuGroupCode}")]
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
