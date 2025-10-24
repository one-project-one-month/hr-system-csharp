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

    public async Task<Result<TaskListResponseModel>> ListAsync(int pageNo, int PageSize)
    {
        var result = await _daTask.List(pageNo, PageSize);
        return result;
    }

    public async Task<Result<TaskCreateResponseModel>> CreateAsync(TaskCreateRequestModel requestModel)
    {
        var result = await _daTask.Create(requestModel);
        return result;
    }

    public async Task<Result<TaskEditResponseModel>> EditAsync(string taskId)
    {
        var result = await _daTask.Edit(taskId);
        return result;
    }

    public async Task<Result<TaskUpdateResponseModel>> UpdateAsync(TaskUpdateRequestModel requestModel)
    {
        var result = await _daTask.Update(requestModel);
        return result;
    }

    public async Task<Result<TaskDeleteResponseModel>> DeleteAsync(string taskCode)
    {
        var result = await _daTask.Delete(taskCode);
        return result;
    }
}
