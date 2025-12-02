using HRSystem.Csharp.Domain.Models.Employee;
using HRSystem.Csharp.Domain.Models.Payroll;

namespace HRSystem.Csharp.Domain.Features.Payroll;

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
        var reqModel = new EmployeeListRequestModel();
        var employees = await _daEmployee.GetEmployeeList(reqModel);
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