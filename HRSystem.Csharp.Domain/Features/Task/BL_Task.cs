using HRSystem.Csharp.Domain.Models.Task;
using HRSystem.Csharp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Csharp.Domain.Features.Task;

public class BL_Task
{
    private readonly DA_Task _daTask;

    public BL_Task(DA_Task daTask)
    {
        _daTask = daTask;
    }

    public async Task<Result<TaskCreateResponseModel>> CreateAsync(TaskCreateRequestModel requestModel)
    {
        var result = await _daTask.Create(requestModel);
        return result;
    }
}
