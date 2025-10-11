using HRSystem.Csharp.Domain.Features.Task;
using HRSystem.Csharp.Domain.Models.Task;
using Microsoft.AspNetCore.Mvc;

namespace HRSystem.Csharp.Api.Controllers;

[ApiController]
[Route("api/task")]
public class TaskController : ControllerBase
{
    private readonly BL_Task _blTask;

    public TaskController(BL_Task blTask)
    {
        _blTask = blTask;
    }

    [HttpGet("list")]
    public async Task<IActionResult> ListAsync()
    {
        var result = await _blTask.ListAsync();
        return Ok(result);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateAsync(TaskCreateRequestModel requestModel)
    {
        var result = await _blTask.CreateAsync(requestModel);
        return Ok(result);
    }

    [HttpGet("edit")]
    public async Task<IActionResult> EditAsync(string taskId)
    {
        var result = await _blTask.EditAsync(taskId);
        return Ok(result);
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdateAsync(TaskUpdateRequestModel requestModel)
    {
        var result = await _blTask.UpdateAsync(requestModel);
        return Ok(result);
    }

    [HttpPost("delete")]
    public async Task<IActionResult> DeleteAsync(string taskId)
    {
        var result = await _blTask.DeleteAsync(taskId);
        return Ok(result);
    }
}
