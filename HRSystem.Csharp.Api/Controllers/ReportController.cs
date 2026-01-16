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

            int pageNo = requestModel.PageNo;
            int pageSize = requestModel.PageSize;

            if (requestModel.PageSize == 0 && requestModel.PageNo == 0)
            {
                requestModel.PageNo = 1;
                requestModel.PageSize = 0;
            }
            else
            {
                pageNo = Math.Max(1, requestModel.PageNo);
                pageSize = Math.Max(1, requestModel.PageSize);

                requestModel.PageNo = pageNo;
                requestModel.PageSize = pageSize;
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
                reportResponse = result.Data!;
            }

            if (reportResponse!.ReportData is null)
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
            var range = worksheet.RangeUsed();
            if (range == null)
                return;

            var table = range.CreateTable();
            table.Theme = XLTableTheme.TableStyleLight9;
            range.FirstRow().Style.Fill.BackgroundColor = XLColor.LightBlue;
            range.FirstRow().Style.Font.Bold = true;
            range.FirstRow().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            worksheet.Columns().AdjustToContents();
        }

        return await _exportService.ExportToExcel(dataList, reportName, ApplyStyling);
    }

    private async Task<byte[]> GeneratePdfExport(object reportData, string title)
    {
        var dataList = ConvertToGenericList(reportData);
        return await _exportService.ExportToPdf(dataList, title);
    }

    private List<Dictionary<string, object>> ConvertToGenericList(object reportData)
    {
        var result = new List<Dictionary<string, object>>();

        if (reportData == null)
            return result;

        var dataType = reportData.GetType();

        if (dataType == typeof(List<Dictionary<string, object>>))
        {
            return (List<Dictionary<string, object>>)reportData;
        }

        if (dataType.IsGenericType && dataType.GetGenericTypeDefinition() == typeof(List<>))
        {
            var itemType = dataType.GetGenericArguments()[0];

            var list = (System.Collections.IEnumerable)reportData;

            foreach (var item in list)
            {
                if (item == null)
                    continue;

                var dict = new Dictionary<string, object>();
                var properties = item.GetType().GetProperties();

                foreach (var prop in properties)
                {
                    try
                    {
                        var value = prop.GetValue(item);
                        dict[prop.Name] = value ?? string.Empty;
                    }
                    catch
                    {
                        dict[prop.Name] = string.Empty;
                    }
                }

                result.Add(dict);
            }
        }

        return result;
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