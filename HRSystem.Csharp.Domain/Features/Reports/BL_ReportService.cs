namespace HRSystem.Csharp.Domain.Features.Reports;

public class BL_ReportService
{
    private readonly DA_ReportService _daReportService;

    public BL_ReportService(DA_ReportService daReportService)
    {
        _daReportService = daReportService;
    }
    
}