using HRSystem.Csharp.Domain.Models.Payroll;
using HRSystem.Csharp.Shared.Services;

namespace HRSystem.Csharp.Domain.Features.Payroll;

public class DA_Payroll
{
    private readonly AppDbContext _appDbContext;
    private readonly DapperService _dapperService;
    public DA_Payroll(AppDbContext appDbContext, DapperService dapperService)
    {
        _appDbContext = appDbContext;
        _dapperService = dapperService;
    }

    public async Task<PayrollListResponseModel> GetPayrollList(PayrollRequestModel requestModel)
    {

        var allPayrolls = await _appDbContext.TblPayrolls
            .Where(p => p.EmployeeCode == requestModel.EmployeeCode)
            .Select(p => 
                new PayrollResponseModel
                {
                    PayrollId = p.PayrollId,
                    PayrollCode = p.PayrollCode,
                    EmployeeCode = p.EmployeeCode,
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

    }

}