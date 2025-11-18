using HRSystem.Csharp.Domain.Features.CheckInOut;
using HRSystem.Csharp.Domain.Models.CheckInOut;

namespace HRSystem.Csharp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckInOutController : ControllerBase
    {
        private readonly BL_CheckInOut _blCheckInOut;
        public CheckInOutController(BL_CheckInOut blCheckInOut)
        {
            _blCheckInOut = blCheckInOut;
        }

        [HttpPost("CheckIn")]
        public async Task<IActionResult> CheckIn(CheckInRequestModel requestModel)
        {
            var result = await _blCheckInOut.CheckIn(requestModel);
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPut("CheckOut")]
        public async Task<IActionResult> CheckOut(CheckOutRequestModel requestModel)
        {
            var result = await _blCheckInOut.CheckOut(requestModel);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);

        }
    }
}
