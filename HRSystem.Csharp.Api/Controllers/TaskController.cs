using HRSystem.Csharp.Domain.Features.Task;
using HRSystem.Csharp.Domain.Models.Task;
using System.Security.Claims;

namespace HRSystem.Csharp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TaskController : ControllerBase
{
    private readonly BL_Task _blTask;

    public TaskController(BL_Task blTask)
    {
        _blTask = blTask;
    }

    [HttpGet("list")]
    public async Task<IActionResult> ListAsync(string? TaskName, int pageNo = 1, int PageSize = 10)
    {
        var result = await _blTask.ListAsync(TaskName, pageNo, PageSize);
        return Ok(result);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateAsync(TaskCreateRequestModel requestModel)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
            return Unauthorized("Invalid user token.");

        var result = await _blTask.CreateAsync(userId, requestModel);
        return Ok(result);
    }

    [HttpGet("edit")]
    public async Task<IActionResult> EditAsync(string taskId)
    {
        var result = await _blTask.EditAsync(taskId);
        return Ok(result);
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateAsync(TaskUpdateRequestModel requestModel)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
            return Unauthorized("Invalid user token.");

        var result = await _blTask.UpdateAsync(userId, requestModel);
        return Ok(result);
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteAsync(string taskId)
    {
        var result = await _blTask.DeleteAsync(taskId);
        return Ok(result);
    }
}
