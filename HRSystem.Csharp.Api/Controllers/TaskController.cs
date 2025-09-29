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

    [HttpPost("create")]
    public async Task<IActionResult> CreateAsync(TaskCreateRequestModel requestModel)
    {
        var result = await _blTask.CreateAsync(requestModel);
        return Ok(result);
    }
}
