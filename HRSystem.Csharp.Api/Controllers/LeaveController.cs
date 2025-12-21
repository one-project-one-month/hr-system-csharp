using HRSystem.Csharp.Domain.Features.Leave;
using HRSystem.Csharp.Domain.Models.Leave;

namespace HRSystem.Csharp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LeaveController : ControllerBase
{
    private readonly BL_Leave _blLeave;

    public LeaveController(BL_Leave blLeave)
    {
        _blLeave = blLeave;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateLeave(LeaveCreateRequestModel reqModel)
    {
        var result = await _blLeave.CreateLeave(reqModel);

        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("check-leave-available")]
    public async Task<IActionResult> CheckLeaveTypeAvailable(AvailableLeaveRequestModel reqModel)
    {
        var result = await _blLeave.CheckLeaveTypeAvailable(reqModel);

        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
    
    
}