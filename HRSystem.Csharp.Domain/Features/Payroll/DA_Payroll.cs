using Dapper;
using HRSystem.Csharp.Domain.Models.Payroll;
using HRSystem.Csharp.Domain.Models.Project;
using HRSystem.Csharp.Shared.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Csharp.Domain.Features.Payroll
{
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

            var parameters = new DynamicParameters();
            parameters.Add("@EmployeeCode", requestModel.EmployeeCode);

            var allPayrolls = (await _dapperService.QueryStoredProcedureWithMultipleResults<PayrollResponseModel>(
                "GetPayrollList",
                parameters
            )).ToList();

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


}
