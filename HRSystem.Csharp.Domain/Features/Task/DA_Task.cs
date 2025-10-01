using HRSystem.Csharp.Domain.Models.Task;
using HRSystem.Csharp.Shared;
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
            var task = new TblTask()
            {
                TaskId = Guid.NewGuid(),
                TaskCode = Guid.NewGuid().ToString(),
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

            return Result<TaskCreateResponseModel>.Success(null, "Task created successfully.");
        }
        catch (Exception ex)
        {
            return Result<TaskCreateResponseModel>.SystemError(ex.Message);
        }
    }
}
