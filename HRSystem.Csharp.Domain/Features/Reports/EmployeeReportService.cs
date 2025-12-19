using HRSystem.Csharp.Domain.Models.Report;

namespace HRSystem.Csharp.Domain.Features.Reports;

public class EmployeeReportService
{
    private readonly ILogger<EmployeeReportService> _logger;
    private readonly AppDbContext _db;
    private readonly ExportService _exportService;

    public EmployeeReportService(
        ILogger<EmployeeReportService> logger,
        AppDbContext db,
        ExportService exportService)
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
                //case EnumReportType.Payroll:
                //    responseModel = await ProcessPayrollReport(requestModel);
                //    break;

                //case EnumReportType.Backlog:
                //    responseModel = await ProcessBacklogReport(requestModel);
                //    break;

                //case EnumReportType.Attendance:
                //    responseModel = await ProcessAttendanceReport(requestModel);
                //    break;

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
}