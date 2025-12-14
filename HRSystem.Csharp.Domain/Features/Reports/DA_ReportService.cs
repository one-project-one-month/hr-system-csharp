namespace HRSystem.Csharp.Domain.Features.Reports;

public class DA_ReportService
{
    private readonly AppDbContext _appDbContext;
    private readonly ILogger<DA_ReportService> _logger;

    public DA_ReportService(AppDbContext appDbContext, ILogger<DA_ReportService> logger)
    {
        _appDbContext = appDbContext;
        _logger = logger;
    }
}