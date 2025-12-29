using HRSystem.Csharp.Domain.Models.Report;
using HRSystem.Csharp.Domain.Models.Report.Admin;

namespace HRSystem.Csharp.Domain.Features.Reports;

public class AdminReportService
{
    private readonly ILogger<AdminReportService> _logger;
    private readonly AppDbContext _db;

    public AdminReportService(
        ILogger<AdminReportService> logger,
        AppDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    public async Task<Result<ReportResponseModel>> GetAdminReport(ReportRequestModel requestModel)
    {
        try
        {
            if (requestModel.ReportType.IsNullOrEmpty())
            {
                return Result<ReportResponseModel>.ValidationError("ReportType is required.");
            }

            if (!Enum.TryParse<EnumReportType>(requestModel.ReportType, true, out var reportType))
            {
                var validTypes = string.Join(", ", Enum.GetNames(typeof(EnumReportType)));
                return Result<ReportResponseModel>.ValidationError(
                    $"Invalid ReportType: '{requestModel.ReportType}'.");
            }

            ReportResponseModel responseModel;

            switch (reportType)
            {
                case EnumReportType.Project:
                    responseModel = await ProcessProjectReport(requestModel);
                    break;

                case EnumReportType.Location:
                    responseModel = await ProcessLocationReport(requestModel);
                    break;

                case EnumReportType.Attendance:
                    responseModel = await ProcessAttendanceReport(requestModel);
                    break;

                case EnumReportType.Payroll:
                    responseModel = await ProcessPayrollReport(requestModel);
                    break;

                case EnumReportType.Backlog:
                    responseModel = await ProcessBacklogReport(requestModel);
                    break;

                default:
                    return Result<ReportResponseModel>.ValidationError($"Invalid report type: {reportType}");
            }

            return Result<ReportResponseModel>.Success(responseModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error Generating Report!");
            return Result<ReportResponseModel>.SystemError("Error Generating Report!");
        }
    }

    #region Project Report

    private async Task<ReportResponseModel> ProcessProjectReport(ReportRequestModel requestModel)
    {
        var query = _db.TblProjects
            .AsNoTracking()
            .Where(p => p.CreatedAt >= requestModel.FromDate && p.CreatedAt <= requestModel.ToDate);

        if (!string.IsNullOrEmpty(requestModel.Item))
        {
            query = query.Where(p =>
                p.ProjectName.Contains(requestModel.Item) ||
                p.ProjectCode.Contains(requestModel.Item) ||
                p.ProjectStatus.Contains(requestModel.Item));
        }

        if (requestModel.IsExport)
        {
            return await HandleProjectExport(query, requestModel);
        }
        else
        {
            return await HandleProjectListing(query, requestModel);
        }
    }

    private async Task<ReportResponseModel> HandleProjectListing(IQueryable<TblProject> query,
        ReportRequestModel requestModel)
    {
        var totalRecords = await query.CountAsync();
        List<ReportProjectModel> data;

        if (requestModel.PageSize == 0)
        {
            data = await query
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new ReportProjectModel
                {
                    ProjectCode = p.ProjectCode,
                    ProjectName = p.ProjectName,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    ProjectStatus = p.ProjectStatus
                })
                .ToListAsync();

            return new PaginatedResponse<ReportProjectModel>
            {
                Data = data,
                TotalRecords = totalRecords,
                PageNumber = 0,
                PageSize = 0,
                TotalPages = 1
            };
        }
        else
        {
            data = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((requestModel.PageNo - 1) * requestModel.PageSize)
                .Take(requestModel.PageSize)
                .Select(p => new ReportProjectModel
                {
                    ProjectCode = p.ProjectCode,
                    ProjectName = p.ProjectName,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    ProjectStatus = p.ProjectStatus
                })
                .ToListAsync();

            return new PaginatedResponse<ReportProjectModel>
            {
                Data = data,
                TotalRecords = totalRecords,
                PageNumber = requestModel.PageNo,
                PageSize = requestModel.PageSize,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)requestModel.PageSize)
            };
        }
    }

    private async Task<ReportResponseModel> HandleProjectExport(IQueryable<TblProject> query,
        ReportRequestModel requestModel)
    {
        var data = await query
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => new ReportProjectModel
            {
                ProjectCode = p.ProjectCode,
                ProjectName = p.ProjectName,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                ProjectStatus = p.ProjectStatus
            })
            .ToListAsync();

        var totalRecords = data.Count;

        return new ReportResponseModel
        {
            ReportData = data,
            TotalRecords = totalRecords
        };
    }

    #endregion

    #region Location Report

    private async Task<ReportResponseModel> ProcessLocationReport(ReportRequestModel requestModel)
    {
        var query = _db.TblLocations
            .AsNoTracking()
            .Where(p => p.CreatedAt >= requestModel.FromDate && p.CreatedAt <= requestModel.ToDate);

        if (!string.IsNullOrEmpty(requestModel.Item))
        {
            query = query.Where(p =>
                p.Name.Contains(requestModel.Item) ||
                p.Latitude.Contains(requestModel.Item) ||
                p.Longitude.Contains(requestModel.Item));
        }

        if (requestModel.IsExport)
        {
            return await HandleLocationExport(query, requestModel);
        }
        else
        {
            return await HandleLocationListing(query, requestModel);
        }
    }

    private async Task<ReportResponseModel> HandleLocationListing(IQueryable<TblLocation> query,
        ReportRequestModel requestModel)
    {
        var totalRecords = await query.CountAsync();
        List<ReportLocationModel> data;

        if (requestModel.PageSize == 0)
        {
            data = await query
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new ReportLocationModel
                {
                    LocationName = p.Name,
                    Latitude = p.Latitude,
                    Longitude = p.Longitude,
                    Radius = p.Radius
                })
                .ToListAsync();

            return new PaginatedResponse<ReportLocationModel>
            {
                Data = data,
                TotalRecords = totalRecords,
                PageNumber = 0,
                PageSize = 0,
                TotalPages = 1
            };
        }
        else
        {
            data = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((requestModel.PageNo - 1) * requestModel.PageSize)
                .Take(requestModel.PageSize)
                .Select(p => new ReportLocationModel
                {
                    LocationName = p.Name,
                    Latitude = p.Latitude,
                    Longitude = p.Longitude,
                    Radius = p.Radius
                })
                .ToListAsync();

            return new PaginatedResponse<ReportLocationModel>
            {
                Data = data,
                TotalRecords = totalRecords,
                PageNumber = requestModel.PageNo,
                PageSize = requestModel.PageSize,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)requestModel.PageSize)
            };
        }
    }

    private async Task<ReportResponseModel> HandleLocationExport(IQueryable<TblLocation> query,
        ReportRequestModel requestModel)
    {
        var data = await query
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => new ReportLocationModel
            {
                LocationName = p.Name,
                Latitude = p.Latitude,
                Longitude = p.Longitude,
                Radius = p.Radius
            })
            .ToListAsync();

        var totalRecords = data.Count;

        return new ReportResponseModel
        {
            ReportData = data,
            TotalRecords = totalRecords
        };
    }

    #endregion

    #region Attendance Report

    private async Task<ReportResponseModel> ProcessAttendanceReport(ReportRequestModel requestModel)
    {
        var query = _db.TblAttendances
            .AsNoTracking()
            .Where(p => p.CreatedAt >= requestModel.FromDate && p.CreatedAt <= requestModel.ToDate);

        if (!string.IsNullOrEmpty(requestModel.Item))
        {
            query = ApplyAttendanceItemFilter(query, requestModel.Item);
        }

        if (requestModel.IsExport)
        {
            return await HandleAttendanceExport(query, requestModel);
        }
        else
        {
            return await HandleAttendanceListing(query, requestModel);
        }
    }

    private IQueryable<TblAttendance> ApplyAttendanceItemFilter(IQueryable<TblAttendance> query, string item)
    {
        query = query.Where(p =>
            p.EmployeeCode!.Contains(item) ||
            p.WorkingHour.ToString()!.Contains(item) ||
            p.Remark!.Contains(item));

        if (DateTime.TryParse(item, out DateTime dateValue))
        {
            query = query.Where(p =>
                (p.AttendanceDate.HasValue && p.AttendanceDate.Value.Date == dateValue.Date) ||
                (p.CheckInTime.HasValue && p.CheckInTime.Value.Date == dateValue.Date) ||
                (p.CheckOutTime.HasValue && p.CheckOutTime.Value.Date == dateValue.Date) ||
                (p.CreatedAt!.Value.Date == dateValue.Date));
        }

        else if (TimeSpan.TryParse(item, out TimeSpan timeValue))
        {
            query = query.Where(p =>
                (p.CheckInTime.HasValue && p.CheckInTime.Value.TimeOfDay == timeValue) ||
                (p.CheckOutTime.HasValue && p.CheckOutTime.Value.TimeOfDay == timeValue));
        }

        return query;
    }

    private async Task<ReportResponseModel> HandleAttendanceListing(IQueryable<TblAttendance> query,
        ReportRequestModel requestModel)
    {
        var totalRecords = await query.CountAsync();
        List<ReportAttendanceModel> data;

        if (requestModel.PageSize == 0)
        {
            data = await query
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new ReportAttendanceModel
                {
                    EmployeeCode = p.EmployeeCode!,
                    AttendanceDate = p.AttendanceDate ?? DateTime.MinValue,
                    CheckInTime = p.CheckInTime ?? DateTime.MinValue,
                    CheckOutTime = p.CheckOutTime ?? DateTime.MinValue,
                    WorkingHour = p.WorkingHour.ToString()!,
                    Remark = p.Remark!
                })
                .ToListAsync();

            return new PaginatedResponse<ReportAttendanceModel>
            {
                Data = data,
                TotalRecords = totalRecords,
                PageNumber = 0,
                PageSize = 0,
                TotalPages = 1
            };
        }
        else
        {
            data = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((requestModel.PageNo - 1) * requestModel.PageSize)
                .Take(requestModel.PageSize)
                .Select(p => new ReportAttendanceModel
                {
                    EmployeeCode = p.EmployeeCode!,
                    AttendanceDate = p.AttendanceDate ?? DateTime.MinValue,
                    CheckInTime = p.CheckInTime ?? DateTime.MinValue,
                    CheckOutTime = p.CheckOutTime ?? DateTime.MinValue,
                    WorkingHour = p.WorkingHour.ToString()!,
                    Remark = p.Remark!
                })
                .ToListAsync();

            return new PaginatedResponse<ReportAttendanceModel>
            {
                Data = data,
                TotalRecords = totalRecords,
                PageNumber = requestModel.PageNo,
                PageSize = requestModel.PageSize,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)requestModel.PageSize)
            };
        }
    }

    private async Task<ReportResponseModel> HandleAttendanceExport(IQueryable<TblAttendance> query,
        ReportRequestModel requestModel)
    {
        var data = await query
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => new ReportAttendanceModel
            {
                EmployeeCode = p.EmployeeCode!,
                AttendanceDate = p.AttendanceDate ?? DateTime.MinValue,
                CheckInTime = p.CheckInTime ?? DateTime.MinValue,
                CheckOutTime = p.CheckOutTime ?? DateTime.MinValue,
                WorkingHour = p.WorkingHour.ToString()!,
                Remark = p.Remark!
            })
            .ToListAsync();

        var totalRecords = data.Count;

        return new ReportResponseModel
        {
            ReportData = data,
            TotalRecords = totalRecords
        };
    }

    #endregion

    #region Payroll Report

    private async Task<ReportResponseModel> ProcessPayrollReport(ReportRequestModel requestModel)
    {
        var query = _db.TblPayrolls
            .AsNoTracking()
            .Where(p => p.CreatedAt >= requestModel.FromDate && p.CreatedAt <= requestModel.ToDate);

        if (requestModel.IsExport == true)
        {
            return await HandlePayrollExport(query, requestModel);
        }
        else
        {
            return await HandlePayrollListing(query, requestModel);
        }
    }

    private async Task<ReportResponseModel> HandlePayrollListing(IQueryable<TblPayroll> query,
        ReportRequestModel requestModel)
    {
        var totalRecords = await query.CountAsync();
        List<ReportPayrollModel> data;

        if (requestModel.PageSize == 0)
        {
            data = await query
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new ReportPayrollModel
                {
                    PayrollCode = p.PayrollCode,
                    EmployeeName = p.EmployeeCode,
                    PayrollMonth = p.PayrollMonth,
                    TotalWorkingHour = p.TotalWorkingHour.ToString()!,
                    LeaveHour = p.LeaveHour.ToString()!,
                    ActualWorkingHour = p.ActualWorkingHour.ToString()!,
                    BaseSalary = p.BaseSalary.ToString()!,
                    GrossPay = p.GrossPay.ToString()!,
                    Deduction = p.Deduction.ToString()!,
                    NetPay = p.NetPay.ToString()!
                })
                .ToListAsync();

            return new PaginatedResponse<ReportPayrollModel>
            {
                Data = data,
                TotalRecords = totalRecords,
                PageNumber = 0,
                PageSize = 0,
                TotalPages = 1
            };
        }
        else
        {
            data = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((requestModel.PageNo - 1) * requestModel.PageSize)
                .Take(requestModel.PageSize)
                .Select(p => new ReportPayrollModel
                {
                    PayrollCode = p.PayrollCode,
                    EmployeeName = p.EmployeeCode,
                    PayrollMonth = p.PayrollMonth,
                    TotalWorkingHour = p.TotalWorkingHour.ToString()!,
                    LeaveHour = p.LeaveHour.ToString()!,
                    ActualWorkingHour = p.ActualWorkingHour.ToString()!,
                    BaseSalary = p.BaseSalary.ToString()!,
                    GrossPay = p.GrossPay.ToString()!,
                    Deduction = p.Deduction.ToString()!,
                    NetPay = p.NetPay.ToString()!
                })
                .ToListAsync();

            return new PaginatedResponse<ReportPayrollModel>
            {
                Data = data,
                TotalRecords = totalRecords,
                PageNumber = requestModel.PageNo,
                PageSize = requestModel.PageSize,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)requestModel.PageSize)
            };
        }
    }

    private async Task<ReportResponseModel> HandlePayrollExport(IQueryable<TblPayroll> query,
        ReportRequestModel requestModel)
    {
        var data = await query
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => new ReportPayrollModel
            {
                PayrollCode = p.PayrollCode,
                EmployeeName = p.EmployeeCode,
                PayrollMonth = p.PayrollMonth,
                TotalWorkingHour = p.TotalWorkingHour.ToString()!,
                LeaveHour = p.LeaveHour.ToString()!,
                ActualWorkingHour = p.ActualWorkingHour.ToString()!,
                BaseSalary = p.BaseSalary.ToString()!,
                GrossPay = p.GrossPay.ToString()!,
                Deduction = p.Deduction.ToString()!,
                NetPay = p.NetPay.ToString()!
            })
            .ToListAsync();

        var totalRecords = data.Count;

        return new ReportResponseModel
        {
            ReportData = data,
            TotalRecords = totalRecords
        };
    }

    #endregion

    #region Backlog Report

    private async Task<ReportResponseModel> ProcessBacklogReport(ReportRequestModel requestModel)
    {
        var query = _db.TblTasks
            .AsNoTracking()
            .Where(p => p.CreatedAt >= requestModel.FromDate && p.CreatedAt <= requestModel.ToDate);

        if (!string.IsNullOrEmpty(requestModel.Item))
        {
            query = query.Where(p =>
                p.TaskCode.Contains(requestModel.Item) ||
                p.TaskName!.Contains(requestModel.Item) ||
                p.ProjectCode!.Contains(requestModel.Item));
        }

        if (requestModel.IsExport)
        {
            return await HandleBacklogExport(query, requestModel);
        }
        else
        {
            return await HandleBacklogListing(query, requestModel);
        }
    }

    private async Task<ReportResponseModel> HandleBacklogListing(IQueryable<TblTask> query,
        ReportRequestModel requestModel)
    {
        var totalRecords = await query.CountAsync();
        List<ReportBacklogModel> data;

        if (requestModel.PageSize == 0)
        {
            data = await query
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new ReportBacklogModel
                {
                    TaskCode = p.TaskCode,
                    ProjectName = p.ProjectCode!,
                    AssignedTo = p.EmployeeCode!,
                    StartDate = ((DateTime)p.StartDate!).ToString("yyyy-MM-dd"),
                    EndDate = ((DateTime)p.EndDate!).ToString("yyyy-MM-dd"),
                    Status = p.TaskStatus!,
                    WorkingHour = p.WorkingHour.ToString()!,
                    CreatedAt = ((DateTime)p.CreatedAt!).ToString("yyyy-MM-dd"),
                    CreatedBy = p.CreatedBy!
                })
                .ToListAsync();

            return new PaginatedResponse<ReportBacklogModel>
            {
                Data = data,
                TotalRecords = totalRecords,
                PageNumber = 0,
                PageSize = 0,
                TotalPages = 1
            };
        }
        else
        {
            data = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((requestModel.PageNo - 1) * requestModel.PageSize)
                .Take(requestModel.PageSize)
                .Select(p => new ReportBacklogModel
                {
                    TaskCode = p.TaskCode,
                    ProjectName = p.ProjectCode!,
                    AssignedTo = p.EmployeeCode!,
                    StartDate = ((DateTime)p.StartDate!).ToString("yyyy-MM-dd"),
                    EndDate = ((DateTime)p.EndDate!).ToString("yyyy-MM-dd"),
                    Status = p.TaskStatus!,
                    WorkingHour = p.WorkingHour.ToString()!,
                    CreatedAt = ((DateTime)p.CreatedAt!).ToString("yyyy-MM-dd"),
                    CreatedBy = p.CreatedBy!
                })
                .ToListAsync();

            return new PaginatedResponse<ReportBacklogModel>
            {
                Data = data,
                TotalRecords = totalRecords,
                PageNumber = requestModel.PageNo,
                PageSize = requestModel.PageSize,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)requestModel.PageSize)
            };
        }
    }

    private async Task<ReportResponseModel> HandleBacklogExport(IQueryable<TblTask> query,
        ReportRequestModel requestModel)
    {
        var data = await query
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => new ReportBacklogModel
            {
                TaskCode = p.TaskCode,
                ProjectName = p.ProjectCode!,
                AssignedTo = p.EmployeeCode!,
                StartDate = ((DateTime)p.StartDate!).ToString("yyyy-MM-dd"),
                EndDate = ((DateTime)p.EndDate!).ToString("yyyy-MM-dd"),
                Status = p.TaskStatus!,
                WorkingHour = p.WorkingHour.ToString()!,
                CreatedAt = ((DateTime)p.CreatedAt!).ToString("yyyy-MM-dd"),
                CreatedBy = p.CreatedBy!
            })
            .ToListAsync();

        var totalRecords = data.Count;

        return new ReportResponseModel
        {
            ReportData = data,
            TotalRecords = totalRecords
        };
    }

    #endregion
}