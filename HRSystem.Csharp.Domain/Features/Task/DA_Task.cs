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

    public async Task<Result<TaskListResponseModel>> List(int pageNo, int PageSize)
    {
        try
        {
            var tasks = await _db.TblTasks.Where(t => t.DeleteFlag == false)
                .OrderByDescending(t => t.CreatedAt)
                .Skip(pageNo * PageSize)
                .Take(PageSize)
                .ToListAsync();

            if (!tasks.Any() || tasks is null)
            {
                return Result<TaskListResponseModel>.NotFoundError("No tasks found.");
            }

            var model = new TaskListResponseModel()
            {
                Tasks = tasks.Select(t =>
                {
                    var task = TaskModel.FromTblTask(t);
                    task.EmployeeName = _db.TblEmployees
                        .Where(e => e.EmployeeCode == t.EmployeeCode && e.DeleteFlag == false)
                        .Select(e => e.Name)
                        .FirstOrDefault();
                    task.ProjectName = _db.TblProjects
                        .Where(p => p.ProjectCode == t.ProjectCode && p.DeleteFlag == false)
                        .Select(p => p.ProjectName)
                        .FirstOrDefault();
                    return task;
                }).ToList()
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

            var model = TaskEditModel.FromTblTask(task);
            model.EmployeeName = _db.TblEmployees
                .Where(e => e.EmployeeCode == task.EmployeeCode && e.DeleteFlag == false)
                .Select(e => e.Name)
                .FirstOrDefault();
            model.ProjectName = _db.TblProjects
                .Where(p => p.ProjectCode == task.ProjectCode && p.DeleteFlag == false)
                .Select(p => p.ProjectName)
                .FirstOrDefault();

            var result = new TaskEditResponseModel()
            {
                Tasks = model
            };

            return Result<TaskEditResponseModel>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<TaskEditResponseModel>.SystemError(ex.Message);
        }
    }

    public async Task<Result<TaskUpdateResponseModel>> Update(TaskUpdateRequestModel requestModel)
    {
        try
        {
            var task = await _db.TblTasks
                .FirstOrDefaultAsync(t => t.DeleteFlag == false
                && t.TaskId == requestModel.TaskId);

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
