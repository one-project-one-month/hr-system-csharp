using HRSystem.Csharp.Domain.Features.Leave;
using HRSystem.Csharp.Domain.Models.Leave;
using HRSystem.Csharp.Shared;
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

    [HttpGet("list")]
    public async Task<IActionResult> GetAllRequestedLeaves([FromQuery] LeaveListRequestModel requestModel)
    {
        var result = await _blLeave.GetAllRequestedLeaves(requestModel);
        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("list-for-employee")]
    public async Task<IActionResult> GetEmployeeLeaveList([FromQuery] EmployeeLeaveListRequestModel reqModel)
    {
        var result = await _blLeave.GetEmployeeLeaveList(reqModel);
        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("leave-balance/{year}")]
    public async Task<IActionResult> LeaveBalance(int year)
    {
        var result = await _blLeave.GetLeaveBalanceByYearAsync(year);
        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result);
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

    [HttpGet("edit/{leaveCode}")]
    public async Task<IActionResult> GetRoleByCode(string leaveCode)
    {
        if (string.IsNullOrWhiteSpace(leaveCode))
        {
            var response = Result<bool>.ValidationError("Leave code is required!");
            return BadRequest(response);
        }

        var result = await _blLeave.GetLeaveByCode(new LeaveEditRequestModel()
        {
            LeaveCode = leaveCode
        });

        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPut("update/{leaveCode}")]
    public async Task<IActionResult> UpdateLeave(string leaveCode, [FromBody] LeaveUpdateRequestModel reqModel)
    {
        if (string.IsNullOrWhiteSpace(leaveCode))
        {
            var response = Result<bool>.ValidationError("Leave code is required!");
            return BadRequest(response);
        }

        var result = await _blLeave.UpdateLeaveAsync(leaveCode, reqModel);
        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpDelete("delete/{leaveCode}")]
    public async Task<IActionResult> DeleteLeave(string leaveCode)
    {
        if (string.IsNullOrWhiteSpace(leaveCode))
        {
            var response = Result<bool>.ValidationError("Leave code is required!");
            return BadRequest(response);
        }

        var result = await _blLeave.DeleteLeaveAsync(leaveCode);

        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
}