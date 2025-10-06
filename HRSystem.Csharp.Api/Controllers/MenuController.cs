using HRSystem.Csharp.Domain.Features;
using HRSystem.Csharp.Domain.Models;
using HRSystem.Csharp.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HRSystem.Csharp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly BL_Menu _blMenu;
        public MenuController(BL_Menu blMenu)
        {
            _blMenu = blMenu;
        }


        [HttpPost("create-menu")]
        public async Task<IActionResult> CreateMenu(Menu requestMenu)
        {
            var result =await _blMenu.CreateMenuAsync(requestMenu);
            if(result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpDelete("delete-menu/{menuCode}")]
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
}
