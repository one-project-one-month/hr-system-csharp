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

            if (!string.IsNullOrWhiteSpace(reqModel.MonthYear))
            {
                if (DateTime.TryParseExact(reqModel.MonthYear, "yyyy-MM", CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out var parsedDate))
                {
                    var targetMonth = parsedDate.ToString("MMM yyyy", CultureInfo.InvariantCulture);
                    query = query.Where(r => r.PayrollMonth == targetMonth);
                }
                else
                {
                    return Result<PayrollListResponseModel>.SystemError($"Invalid date format.");
                }
            }

            query = query.OrderByDescending(r => r.CreatedAt);

            var payrollSummaryList = query.Select(s => new PayrollResponseModel()
            {
                PayrollSummaryId = s.PayrollSummaryId,
                PayrollSummaryCode = s.PayrollSummaryCode,
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
            _logger.LogError(ex, "Error fetching payroll summary list");
            return Result<PayrollListResponseModel>.SystemError(
                "An error occurred while retrieving payroll summary list.");
        }
    }

    public async Task<Result<PayrollMonthDetailListResponseModel>> PayrollMonthDetailList(
        PayrollMonthDetailListRequestModel reqModel)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(reqModel.PayrollSummaryCode))
            {
                return Result<PayrollMonthDetailListResponseModel>.ValidationError("Payroll Summary Code is required!");
            }

            var query = from p in _appDbContext.TblPayrolls.AsNoTracking()
                where p.PayrollSummaryCode != null && p.PayrollSummaryCode == reqModel.PayrollSummaryCode
                join e in _appDbContext.TblEmployees.AsNoTracking()
                    on p.EmployeeCode equals e.EmployeeCode
                select new { Payroll = p, Employee = e };

            if (!string.IsNullOrWhiteSpace(reqModel.EmployeeName))
            {
                query = query.Where(x => x.Employee.Name.Contains(reqModel.EmployeeName));
            }

            query = query.OrderByDescending(x => x.Payroll.CreatedAt);

            var payrollDetailList = query.Select(x => new PayrollMonthDetailResponseModel()
            {
                PayrollId = x.Payroll.PayrollId,
                PayrollCode = x.Payroll.PayrollCode,
                EmployeeCode = x.Payroll.EmployeeCode,
                EmployeeName = x.Employee.Name,
                PayrollDate = x.Payroll.PayrollDate,
                Status = x.Payroll.Status,
                TotalWorkingHour = x.Payroll.TotalWorkingHour,
                LeaveHour = x.Payroll.LeaveHour,
                ActualWorkingHour = x.Payroll.ActualWorkingHour,
                BaseSalary = x.Payroll.BaseSalary,
                Allowance = x.Payroll.Allowance,
                GrossPay = x.Payroll.GrossPay,
                Deduction = x.Payroll.Deduction,
                Tax = x.Payroll.Tax,
                Bonus = x.Payroll.Bonus,
                NetPay = x.Payroll.NetPay,
                CreatedBy = x.Payroll.CreatedBy,
                CreatedAt = x.Payroll.CreatedAt
            });

            var pagedResult = await payrollDetailList.GetPagedResultAsync(
                reqModel.PageNo, reqModel.PageSize);

            var result = new PayrollMonthDetailListResponseModel()
            {
                Items = pagedResult.Items,
                TotalCount = pagedResult.TotalCount,
                PageNo = reqModel.PageNo,
                PageSize = reqModel.PageSize
            };

            return Result<PayrollMonthDetailListResponseModel>.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching payroll detail list");
            return Result<PayrollMonthDetailListResponseModel>.SystemError(
                "An error occurred while retrieving payroll detail list.");
        }
    }

    public async Task<Result<EmployeePayrollListResponseModel>> GetPayrollListForEmployeeAsync(
        EmployeePayrollListRequestModel reqModel)
    {
        try
        {
            var query = _appDbContext.TblPayrolls
                .AsNoTracking()
                .Where(p => p.EmployeeCode == UserCode);

            if (!string.IsNullOrWhiteSpace(reqModel.MonthYear))
            {
                if (DateTime.TryParseExact(reqModel.MonthYear, "yyyy-MM", CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out var parsedDate))
                {
                    var targetMonth = parsedDate.ToString("MMM yyyy", CultureInfo.InvariantCulture);
                    query = query.Where(r => r.PayrollMonth == targetMonth);
                }
                else
                {
                    return Result<EmployeePayrollListResponseModel>.SystemError($"Invalid date format.");
                }
            }

            query = query.OrderByDescending(p => p.PayrollDate);

            var payrollList = query.Select(p => new EmployeePayrollResponseModel()
            {
                PayrollId = p.PayrollId,
                PayrollCode = p.PayrollCode,
                PayrollSummaryCode = p.PayrollSummaryCode,
                PayrollDate = p.PayrollDate,
                PayrollMonth = p.PayrollMonth,
                Status = p.Status,
                TotalWorkingHour = p.TotalWorkingHour,
                LeaveHour = p.LeaveHour,
                ActualWorkingHour = p.ActualWorkingHour,
                BaseSalary = p.BaseSalary,
                Allowance = p.Allowance,
                GrossPay = p.GrossPay,
                Deduction = p.Deduction,
                Tax = p.Tax,
                Bonus = p.Bonus,
                NetPay = p.NetPay,
                CreatedAt = p.CreatedAt
            });

            var pagedResult = await payrollList.GetPagedResultAsync(reqModel.PageNo, reqModel.PageSize);

            var result = new EmployeePayrollListResponseModel()
            {
                Items = pagedResult.Items,
                TotalCount = pagedResult.TotalCount,
                PageNo = reqModel.PageNo,
                PageSize = reqModel.PageSize
            };

            return Result<EmployeePayrollListResponseModel>.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching employee payroll list");
            return Result<EmployeePayrollListResponseModel>.SystemError(
                "An error occurred while retrieving payroll list.");
        }
    }

    public async Task<Result<MonthlyPayrollChartResponseModel>> GetMonthlyPayrollChartAsync(
        MonthlyPayrollChartRequestModel reqModel)
    {
        try
        {
            var payrolls = await _appDbContext.TblPayrolls
                .Where(p => p.EmployeeCode == "EMP_251114_0001" &&
                            p.PayrollMonth.EndsWith(reqModel.Year.ToString()))
                .ToListAsync();

            var grouped = payrolls
                .GroupBy(p => p.PayrollMonth)
                .ToDictionary(
                    g => g.Key,
                    g => new MonthlyPayrollChartModel
                    {
                        Month = g.Key,
                        NetPay = g.Sum(x => x.NetPay),
                        GrossPay = g.Sum(x => x.GrossPay),
                        Deduction = g.Sum(x => x.Deduction)
                    });

            var monthlyList = new List<MonthlyPayrollChartModel>();

            for (int m = 1; m <= 12; m++)
            {
                var monthKey = new DateTime(reqModel.Year, m, 1)
                    .ToString("MMM yyyy", CultureInfo.InvariantCulture);

                var shortMonth = new DateTime(reqModel.Year, m, 1)
                    .ToString("MMM", CultureInfo.InvariantCulture);

                if (grouped.ContainsKey(monthKey))
                {
                    // monthlyList.Add(grouped[monthKey]);
                    var item = grouped[monthKey];
                    item.Month = shortMonth;
                    monthlyList.Add(item);
                }
                else
                {
                    monthlyList.Add(new MonthlyPayrollChartModel
                    {
                        Month = shortMonth,
                        NetPay = 0,
                        GrossPay = 0,
                        Deduction = 0
                    });
                }
            }

            var response = new MonthlyPayrollChartResponseModel
            {
                PayrollChart = monthlyList
            };

            return Result<MonthlyPayrollChartResponseModel>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating payroll chart");
            return Result<MonthlyPayrollChartResponseModel>.SystemError(
                "Unable to generate payroll chart.");
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