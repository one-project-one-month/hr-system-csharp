using HRSystem.Csharp.Domain.Models.Employee;
using HRSystem.Csharp.Domain.Models.Payroll;
using HRSystem.Csharp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Csharp.Domain.Features.Payroll
{
    public class BL_Payroll
    {
        private readonly DA_Payroll _daPayroll;
        private readonly DA_Employee _daEmployee;

        public BL_Payroll (DA_Payroll daPayroll, DA_Employee daEmployee)
        {
            _daPayroll = daPayroll;
            _daEmployee = daEmployee;
        }

        public async Task<Result<List<PayrollListResponseModel>>> GetPayrollList(PayrollRequestModel requestModel)
        { 
            var EmployeeListRequestModel = new EmployeeListRequestModel();
            var employees = await _daEmployee.GetAllEmployee(EmployeeListRequestModel);
            List<PayrollListResponseModel> payrollList = new List<PayrollListResponseModel>();
            foreach(var employee in employees.Data.Items)
            {
                requestModel.EmployeeCode = employee.EmployeeCode;
                var result = await _daPayroll.GetPayrollList(requestModel);
                payrollList.Add(result);
            }
            return Result<List<PayrollListResponseModel>>.Success(payrollList);
        } 
    }
}
