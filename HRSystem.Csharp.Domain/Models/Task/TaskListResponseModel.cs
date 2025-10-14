using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Csharp.Domain.Models.Task;

public class TaskListResponseModel
{
    public List<TaskModel>? Tasks { get; set; }
}

public class TaskModel
{
    public Guid TaskId { get; set; }

    public string? TaskCode { get; set; }

    public string? TaskName { get; set; }

    public string? EmployeeCode { get; set; }

    public string? ProjectCode { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? TaskStatus { get; set; }

    public decimal? WorkingHour { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public static TaskModel FromTblTask(TblTask tblTask)
    {
        return new TaskModel()
        {
            TaskId = tblTask.TaskId,
            TaskCode = tblTask.TaskCode,
            TaskName = tblTask.TaskName,
            EmployeeCode = tblTask.EmployeeCode,
            ProjectCode = tblTask.ProjectCode,
            StartDate = tblTask.StartDate,
            EndDate = tblTask.EndDate,
            TaskStatus = tblTask.TaskStatus,
            WorkingHour = tblTask.WorkingHour,
            CreatedAt = tblTask.CreatedAt,
            CreatedBy = tblTask.CreatedBy,
            ModifiedAt = tblTask.ModifiedAt,
            ModifiedBy = tblTask.ModifiedBy
        };
    }
}