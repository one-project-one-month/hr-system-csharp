using HRSystem.Csharp.Domain.Models.Task;
using HRSystem.Csharp.Shared;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Csharp.Domain.Features.Task;

public class DA_Task
{
    private readonly AppDbContext _db;

    public DA_Task(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Result<TaskListResponseModel>> List()
    {
        try
        {
            var tasks = await _db.TblTasks.Where(t => t.DeleteFlag == false)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

            if (!tasks.Any() || tasks is null)
            {
                return Result<TaskListResponseModel>.NotFoundError("No tasks found.");
            }

            var model = new TaskListResponseModel()
            {
                Tasks = tasks.Select(t => TaskModel.FromTblTask(t)).ToList()
            };

            return Result<TaskListResponseModel>.Success(model);
        }
        catch (Exception ex)
        {
            return Result<TaskListResponseModel>.SystemError(ex.Message);
        }
    }

    public async Task<Result<TaskCreateResponseModel>> Create(TaskCreateRequestModel requestModel)
    {
        try
        {
            var taskCode = await GenerateSequenceCodeAsync("T");
            var task = new TblTask()
            {
                TaskId = Guid.NewGuid(),
                TaskCode = taskCode,
                EmployeeCode = requestModel.EmployeeCode,
                ProjectCode = requestModel.ProjectCode,
                TaskName = requestModel.TaskName,
                TaskDescription = requestModel.TaskDescription,
                StartDate = requestModel.StartDate,
                EndDate = requestModel.EndDate,
                TaskStatus = requestModel.TaskStatus,
                WorkingHour = requestModel.WorkingHour,
                DeleteFlag = false,
                CreatedAt = DateTime.UtcNow,
            };

            await _db.TblTasks.AddAsync(task);
            await _db.SaveChangesAsync();

            UpdateSequenceNoAsync("T", taskCode.Substring(1));

            return Result<TaskCreateResponseModel>.Success(null, "Task created successfully.");
        }
        catch (Exception ex)
        {
            return Result<TaskCreateResponseModel>.SystemError(ex.Message);
        }
    }

    public async Task<Result<TaskEditResponseModel>> Edit(string taskId)
    {
        if (taskId.IsNullOrEmpty())
        {
            return Result<TaskEditResponseModel>.BadRequestError("TaskId is required.");
        }
        try
        {
            var task = await _db.TblTasks.FirstOrDefaultAsync(t => t.TaskId.ToString() == taskId && t.DeleteFlag == false);

            if (task is null)
            {
                return Result<TaskEditResponseModel>.NotFoundError("Task not found.");
            }

            var model = new TaskEditResponseModel()
            {
                Tasks = TaskEditModel.FromTblTask(task)
            };

            return Result<TaskEditResponseModel>.Success(model);
        }
        catch (Exception ex)
        {
            return Result<TaskEditResponseModel>.SystemError(ex.Message);
        }
    }

    public async Task<Result<TaskUpdateResponseModel>> Update(TaskUpdateRequestModel requestModel)
    {
        if (requestModel.TaskCode.IsNullOrEmpty())
        {
            return Result<TaskUpdateResponseModel>.BadRequestError("Task Code is required.");
        }

        try
        {
            var task = await _db.TblTasks
                .FirstOrDefaultAsync(t => t.DeleteFlag == false
                && t.TaskCode == requestModel.TaskCode);

            if (task is null)
            {
                return Result<TaskUpdateResponseModel>.NotFoundError("Task not found.");
            }

            task.EmployeeCode = requestModel.EmployeeCode;
            task.ProjectCode = requestModel.ProjectCode;
            task.TaskName = requestModel.TaskName;
            task.TaskDescription = requestModel.TaskDescription;
            task.StartDate = requestModel.StartDate;
            task.EndDate = requestModel.EndDate;
            task.TaskStatus = requestModel.TaskStatus;
            task.WorkingHour = requestModel.WorkingHour;
            task.ModifiedAt = DateTime.UtcNow;

            _db.Entry(task).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            return Result<TaskUpdateResponseModel>.Success(null, "Task is updated successfully.");
        }
        catch (Exception ex)
        {
            return Result<TaskUpdateResponseModel>.SystemError(ex.Message);
        }
    }

    public async Task<Result<TaskDeleteResponseModel>> Delete(string taskId)
    {
        if (taskId.IsNullOrEmpty())
        {
            return Result<TaskDeleteResponseModel>.BadRequestError("TaskId is required.");
        }

        try
        {
            var task = await _db.TblTasks.FirstOrDefaultAsync(t => t.TaskId.ToString() == taskId && t.DeleteFlag == false);

            if (task is null)
            {
                return Result<TaskDeleteResponseModel>.NotFoundError("Task not found.");
            }

            task.DeleteFlag = true;
            task.ModifiedAt = DateTime.UtcNow;
            _db.Entry(task).State = EntityState.Modified;
            _db.SaveChanges();

            return Result<TaskDeleteResponseModel>.Success(null, "Task is deleted successfully.");
        }

        catch (Exception ex)
        {
            return Result<TaskDeleteResponseModel>.SystemError(ex.Message);
        }
    }

    public async Task<string> GenerateSequenceCodeAsync(string uniqueName)
    {
        var sequence = await _db.TblSequences
            .FirstOrDefaultAsync(s => s.UniqueName == uniqueName);

        if (sequence is null)
        {
            throw new Exception("Sequence not found.");
        }

        var sequenceNo = Int32.Parse(sequence.SequenceNo!) + 1;

        var sequenceCode = uniqueName + sequenceNo.ToString("D6");
        return sequenceCode;
    }

    public void UpdateSequenceNoAsync(string uniqueName, string sequenceNo)
    {
        var sequence = _db.TblSequences
            .FirstOrDefault(s => s.UniqueName == uniqueName);

        if (sequence is null)
        {
            throw new Exception("Sequence not found.");
        }

        sequence.SequenceNo = sequenceNo;

        _db.Entry(sequence).State = EntityState.Modified;
        _db.SaveChanges();
    }
}
