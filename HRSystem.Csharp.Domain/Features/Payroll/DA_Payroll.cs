using HRSystem.Csharp.Domain.Models.Payroll;
using HRSystem.Csharp.Shared.Services;
using Sprache;

namespace HRSystem.Csharp.Domain.Features.Payroll;

public class DA_Payroll : AuthorizationService
{
    private readonly AppDbContext _appDbContext;
    private readonly DapperService _dapperService;
    private readonly ILogger<DA_Payroll> _logger;

    public DA_Payroll(IHttpContextAccessor httpContextAccessor,
        AppDbContext appDbContext,
        DapperService dapperService, ILogger<DA_Payroll> logger) : base(httpContextAccessor)
    {
        _appDbContext = appDbContext;
        _dapperService = dapperService;
        _logger = logger;
    }

    public async Task<Result<bool>> ProcessPayroll(PayrollProcessRequestModel reqModel)
    {
        try
        {
            var parameters = new
            {
                PayrollMonth = reqModel.PayrollMonth,
                CreatedBy = UserCode
            };
            
            await _dapperService.ExecuteAsync(
                "sp_ProcessPayroll",
                parameters,
                CommandType.StoredProcedure);

            return Result<bool>.Success("Payroll processed successfully!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString(), "Error processing payroll!");
            return Result<bool>.SystemError("Payroll didn't process properly");
        }
    }

    public async Task<Result<PayrollListResponseModel>> PayrollList(PayrollListRequestModel reqModel)
    {
        try
        {
            var query = _appDbContext.TblPayrollSummaries
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(reqModel.MonthName))
            {
                query = query.Where(r => r.PayrollMonth.ToLower().Contains(reqModel.MonthName.ToLower()));
            }

            query = query.OrderByDescending(r => r.CreatedAt);

            var payrollSummaryList = query.Select(s => new PayrollResponseModel()
            {
                PayrollSummaryId = s.PayrollSummaryId,
                PayrollMonth = s.PayrollMonth,
                TotalWorkingDays = s.TotalWorkingDays,
                EmployeeCount = s.EmployeeCount,
                TotalWorkingHours = s.TotalWorkingHours,
                TotalLeaveHours = s.TotalLeaveHours,
                TotalActualWorkingHours = s.TotalActualWorkingHours,
                TotalBaseSalary = s.TotalBaseSalary,
                TotalNetPay = s.TotalNetPay,
                CreatedBy = s.CreatedBy,
                CreatedAt = s.CreatedAt
            });

            var pagedResult = await payrollSummaryList.GetPagedResultAsync(reqModel.PageNo, reqModel.PageSize);

            var result = new PayrollListResponseModel()
            {
                Items = pagedResult.Items,
                TotalCount = pagedResult.TotalCount,
                PageNo = reqModel.PageNo,
                PageSize = reqModel.PageSize
            };

            return Result<PayrollListResponseModel>.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching payroll list");
            return Result<PayrollListResponseModel>.SystemError(
                "An error occurred while retrieving payroll summary list.");
        }
    }

    /*public async Task<PayrollListResponseModel> GetPayrollList(PayrollRequestModel requestModel)
    {
        var allPayrolls = await _appDbContext.TblPayrolls
            .Where(p => p.EmployeeCode == requestModel.EmployeeCode)
            .Select(p =>
                new PayrollResponseModel
                {
                    PayrollId = p.PayrollId,
                    PayrollCode = p.PayrollCode,
                    EmployeeCode = p.EmployeeCode!,
                    PayrollDate = p.PayrollDate,
                    TotalWorkingHour = p.TotalWorkingHour,
                    BaseSalary = p.BaseSalary,
                    Bonus = p.Bonus,
                    GrossPay = p.GrossPay,
                    Deduction = p.Deduction,
                    Tax = p.Tax,
                    NetPay = p.NetPay,
                }
            ).ToListAsync();

        // Optional filtering and pagination in memory
        var filtered = allPayrolls
            .Where(p => string.IsNullOrEmpty(requestModel.EmployeeName) ||
                        (p.EmployeeName ?? "").Contains(requestModel.EmployeeName, StringComparison.OrdinalIgnoreCase))
            .ToList();

        var paged = filtered
            .Skip((requestModel.PageNo - 1) * requestModel.PageSize)
            .Take(requestModel.PageSize)
            .ToList();

        return new PayrollListResponseModel
        {
            Items = paged,
            TotalCount = paged.Count(),
            PageNo = requestModel.PageNo,
            PageSize = requestModel.PageSize
        };
    }*/
}