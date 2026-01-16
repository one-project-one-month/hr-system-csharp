using HRSystem.Csharp.Domain.Models.Report;
using HRSystem.Csharp.Domain.Models.Report.Admin;

namespace HRSystem.Csharp.Domain.Features.Reports;

public class EmployeeReportService : AuthorizationService
{
    private readonly ILogger<EmployeeReportService> _logger;
    private readonly AppDbContext _db;
    private readonly ExportService _exportService;

    public EmployeeReportService(
        IHttpContextAccessor httpContextAccessor,
        ILogger<EmployeeReportService> logger,
        AppDbContext db,
        ExportService exportService) : base(httpContextAccessor)
    {
        _logger = logger;
        _db = db;
        _exportService = exportService;
    }

    public async Task<Result<ReportResponseModel>> GetEmployeeReport(ReportRequestModel requestModel)
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
                case EnumReportType.Payroll:
                    responseModel = await ProcessPayrollReport(requestModel);
                    break;

                case EnumReportType.Backlog:
                    responseModel = await ProcessBacklogReport(requestModel);
                    break;

                case EnumReportType.Attendance:
                    responseModel = await ProcessAttendanceReport(requestModel);
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
        List<EmployeeReportAttendanceModel> data;

        if (requestModel.PageSize == 0)
        {
            data = await query
                .OrderByDescending(p => p.CreatedAt)
                .Where(p => p.EmployeeCode == UserCode)
                .Select(p => new EmployeeReportAttendanceModel
                {
                    AttendanceDate = p.AttendanceDate ?? DateTime.MinValue,
                    CheckInTime = p.CheckInTime ?? DateTime.MinValue,
                    CheckOutTime = p.CheckOutTime ?? DateTime.MinValue,
                    WorkingHour = p.WorkingHour.ToString()!,
                    Remark = p.Remark!
                })
                .ToListAsync();

            return new PaginatedResponse<EmployeeReportAttendanceModel>
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
                .Where(p => p.EmployeeCode == UserCode)
                .Select(p => new EmployeeReportAttendanceModel
                {
                    AttendanceDate = p.AttendanceDate ?? DateTime.MinValue,
                    CheckInTime = p.CheckInTime ?? DateTime.MinValue,
                    CheckOutTime = p.CheckOutTime ?? DateTime.MinValue,
                    WorkingHour = p.WorkingHour.ToString()!,
                    Remark = p.Remark!
                })
                .ToListAsync();

            return new PaginatedResponse<EmployeeReportAttendanceModel>
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
            .Where(p => p.EmployeeCode == UserCode)
            .Select(p => new EmployeeReportAttendanceModel
            {
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
        List<EmployeeReportPayrollModel> data;

        if (requestModel.PageSize == 0)
        {
            data = await query
                .OrderByDescending(p => p.CreatedAt)
                .Where(p => p.EmployeeCode == UserCode)
                .Select(p => new EmployeeReportPayrollModel
                {
                    PayrollCode = p.PayrollCode,
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

            return new PaginatedResponse<EmployeeReportPayrollModel>
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
                .Where(p => p.EmployeeCode == UserCode)
                .Select(p => new EmployeeReportPayrollModel
                {
                    PayrollCode = p.PayrollCode,
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

            return new PaginatedResponse<EmployeeReportPayrollModel>
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
            .Where(p => p.EmployeeCode == UserCode)
            .Select(p => new EmployeeReportPayrollModel
            {
                PayrollCode = p.PayrollCode,
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
                .Where(p => p.EmployeeCode == UserCode)
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
                .Where(p => p.EmployeeCode == UserCode)
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
            .Where(p => p.EmployeeCode == UserCode)
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