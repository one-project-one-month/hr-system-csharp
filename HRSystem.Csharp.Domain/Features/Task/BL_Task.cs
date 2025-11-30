using HRSystem.Csharp.Domain.Models.Task;

namespace HRSystem.Csharp.Domain.Features.Task;

public class BL_Task
{
    private readonly DA_Task _daTask;

    public BL_Task(DA_Task daTask)
    {
        _daTask = daTask;
    }

    public async Task<Result<TaskListResponseModel>> ListAsync(string TaskName, int pageNo, int PageSize)
    {
        var result = await _daTask.List(TaskName, pageNo, PageSize);
        return result;
    }

    public async Task<Result<TaskCreateResponseModel>> CreateAsync(string userId, TaskCreateRequestModel requestModel)
    {
        var result = await _daTask.Create(userId, requestModel);
        return result;
    }

    public async Task<Result<TaskEditResponseModel>> EditAsync(string taskId)
    {
        var result = await _daTask.Edit(taskId);
        return result;
    }

    public async Task<Result<TaskUpdateResponseModel>> UpdateAsync(string userId, TaskUpdateRequestModel requestModel)
    {
        var result = await _daTask.Update(userId,requestModel);
        return result;
    }

    public async Task<Result<TaskDeleteResponseModel>> DeleteAsync(string taskCode)
    {
        var result = await _daTask.Delete(taskCode);
        return result;
    }
}