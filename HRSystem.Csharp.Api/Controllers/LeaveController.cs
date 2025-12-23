using HRSystem.Csharp.Domain.Features.Leave;
using HRSystem.Csharp.Domain.Models.Leave;
using Microsoft.AspNetCore.Authorization;

namespace HRSystem.Csharp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
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

    [HttpPost("approve-leave")]
    public async Task<IActionResult> ApproveLeave(LeaveApproveRequestModel reqModel)
    {
        var result = await _blLeave.ApproveLeaveAsync(reqModel);

        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("reject-leave")]
    public async Task<IActionResult> RejectLeave(LeaveRejectRequestModel reqModel)
    {
        var result = await _blLeave.RejectLeaveAsync(reqModel);

        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
}