using ClosedXML.Excel;
using HRSystem.Csharp.Domain.Features.Reports;
using HRSystem.Csharp.Domain.Models.Report;

namespace HRSystem.Csharp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReportController : ControllerBase
{
    private readonly AdminReportService _adminReportService;
    private readonly EmployeeReportService _employeeReportService;
    private readonly ExportService _exportService;

    public ReportController(
        AdminReportService adminReportService,
        EmployeeReportService employeeReportService,
        ExportService exportService)
    {
        _adminReportService = adminReportService;
        _employeeReportService = employeeReportService;
        _exportService = exportService;
    }

    [HttpPost("list")]
    public async Task<IActionResult> GetReportList([FromBody] ReportRequestModel requestModel)
    {
        try
        {
            requestModel.IsExport = false;

            if (requestModel.PageSize <= 0)
            {
                requestModel.PageSize = 10;
            }

            var result = await _adminReportService.GetAdminReport(requestModel);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Data);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error getting report list: {ex.Message}");
        }
    }

    [HttpPost("export")]
    public async Task<IActionResult> ExportReport([FromBody] ExportRequestModel requestModel)
    {
        var reportResponse = new ReportResponseModel();

        try
        {
            if (requestModel.ReportRequest == null)
            {
                return BadRequest("Report request is required.");
            }

            requestModel.ReportRequest.IsExport = true;

            if (requestModel.ReportType.Contains("admin", StringComparison.OrdinalIgnoreCase))
            {
                var result = await _adminReportService.GetAdminReport(requestModel.ReportRequest);
                if (!result.IsSuccess)
                {
                    return BadRequest(result.Message);
                }
                reportResponse = result.Data!;
            }
            else
            {
                var result = await _employeeReportService.GetEmployeeReport(requestModel.ReportRequest);
                if (!result.IsSuccess)
                {
                    return BadRequest(result.Message);
                }
                reportResponse = result.Data as ReportResponseModel;
            }

            if (reportResponse.ReportData is null)
            {
                return NotFound("No data found for export.");
            }

            byte[] fileContent;
            string contentType;
            string fileExtension;
            string sheetName = $"{requestModel.ReportName}_{DateTime.Now:yyyyMMddHHmmss}";

            switch (requestModel.Format.ToLower())
            {
                case "csv":
                    fileContent = await GenerateCsvExport(reportResponse.ReportData);
                    contentType = "text/csv";
                    fileExtension = "csv";
                    break;

                case "pdf":
                    fileContent = await GeneratePdfExport(reportResponse.ReportData, requestModel.ReportName);
                    contentType = "application/pdf";
                    fileExtension = "pdf";
                    break;

                case "xlsx":
                case "excel":
                default:
                    fileContent = await GenerateExcelExport(reportResponse.ReportData, requestModel.ReportName);
                    contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    fileExtension = "xlsx";
                    break;
            }

            return File(fileContent, contentType, $"{sheetName}.{fileExtension}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Export failed: {ex.Message}");
        }
    }

    #region Export Generation Methods

    private async Task<byte[]> GenerateCsvExport(object reportData)
    {
        var dataList = ConvertToGenericList(reportData);
        return await _exportService.ExportToCsv(dataList);
    }

    private async Task<byte[]> GenerateExcelExport(object reportData, string reportName)
    {
        var dataList = ConvertToGenericList(reportData);

        void ApplyStyling(IXLWorksheet worksheet)
        {
            worksheet.Columns().AdjustToContents();

            var headerRange = worksheet.Range(1, 1, 1, worksheet.Columns().Count());
            headerRange.Style.Fill.BackgroundColor = XLColor.LightBlue;
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            var dataRange = worksheet.Range(1, 1, worksheet.Rows().Count(), worksheet.Columns().Count());
            dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        }

        return await _exportService.ExportToExcel(dataList, reportName, ApplyStyling);
    }

    private async Task<byte[]> GeneratePdfExport(object reportData, string title)
    {
        var dataList = ConvertToGenericList(reportData);
        return await _exportService.ExportToPdf(dataList, title);
    }

    private List<object> ConvertToGenericList(object reportData)
    {
        if (reportData is List<object> list)
            return list;

        if (reportData is System.Collections.IEnumerable enumerable)
        {
            var result = new List<object>();
            foreach (var item in enumerable)
            {
                result.Add(item);
            }
            return result;
        }

        return new List<object> { reportData };
    }

    #endregion
}

public class ExportRequestModel
{
    public string Format { get; set; } = "excel";
    public string ReportType { get; set; } = string.Empty;
    public string ReportName { get; set; } = string.Empty;
    public ReportRequestModel ReportRequest { get; set; } = new ReportRequestModel();
}