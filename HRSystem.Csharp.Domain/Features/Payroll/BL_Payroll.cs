using HRSystem.Csharp.Domain.Models.Payroll;

namespace HRSystem.Csharp.Domain.Features.Payroll;

public class BL_Payroll
{
    private readonly DA_Payroll _daPayroll;
    private readonly DA_Employee _daEmployee;
    private readonly ILogger<BL_Payroll> _logger;

    public BL_Payroll(DA_Payroll daPayroll, DA_Employee daEmployee, ILogger<BL_Payroll> logger)
    {
        _daPayroll = daPayroll;
        _daEmployee = daEmployee;
        _logger = logger;
    }

    public async Task<Result<bool>> ProcessPayroll(PayrollProcessRequestModel reqModel)
    {
        try
        {
            var result = await _daPayroll.ProcessPayroll(reqModel);
            return result;
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
            return Result<bool>.SystemError("Error occured while processing payroll!");
        }
    }

    public async Task<Result<PayrollListResponseModel>> PayrollList(PayrollListRequestModel reqModel)
    {
        try
        {
            var result = await _daPayroll.PayrollList(reqModel);
            return result;
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString(), "Error fetching payroll list.");
            return Result<PayrollListResponseModel>.SystemError("Error fetching payroll list!");
        }
    }

    public async Task<Result<PayrollMonthDetailListResponseModel>> PayrollMonthDetailList(
        PayrollMonthDetailListRequestModel reqModel)
    {
        try
        {
            var result = await _daPayroll.PayrollMonthDetailList(reqModel);
            return result;
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString(), "Error fetching payroll list.");
            return Result<PayrollMonthDetailListResponseModel>.SystemError("Error fetching payroll list!");
        }
    }

    public async Task<Result<EmployeePayrollListResponseModel>> EmployeePayrollList(
        EmployeePayrollListRequestModel reqModel)
    {
        try
        {
            var result = await _daPayroll.GetPayrollListForEmployeeAsync(reqModel);
            return result;
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString(), "Error fetching payroll list.");
            return Result<EmployeePayrollListResponseModel>.SystemError("Error fetching payroll list!");
        }
    }
    
    /*public async Task<Result<EmployeePayrollListResponseModel>> EmployeePayrollChart(
        EmployeePayrollListRequestModel reqModel)
    {
        try
        {
            var result = await _daPayroll.GetPayrollListForEmployeeAsync(reqModel);
            return result;
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString(), "Error fetching payroll list.");
            return Result<EmployeePayrollListResponseModel>.SystemError("Error fetching payroll list!");
        }
    }*/

    /*public async Task<Result<List<PayrollListResponseModel>>> GetPayrollList(PayrollRequestModel requestModel)
    {
        var reqModel = new EmployeeListRequestModel();
        var employees = await _daEmployee.GetEmployeeList(reqModel);
        List<PayrollListResponseModel> payrollList =  []
        ;
        foreach (var employee in employees.Data!.Items!)
        {
            requestModel.EmployeeCode = employee.EmployeeCode;
            var result = await _daPayroll.GetPayrollList(requestModel);
            payrollList.Add(result);
        }

        return Result<List<PayrollListResponseModel>>.Success(payrollList);
    }*/
}