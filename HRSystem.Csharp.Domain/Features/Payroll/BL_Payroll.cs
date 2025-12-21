using System.Text.RegularExpressions;
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
            #region Validate PayrollMonth Format

            var validation = ValidatePayrollMonth(reqModel.PayrollMonth);
            if (!validation.IsSuccess)
            {
                return validation;
            }

            #endregion

            #region Check Payroll has already processed for the given month

            var alreadyProcessed = await _daPayroll.ExistsForMonthAsync(reqModel.PayrollMonth);

            if (alreadyProcessed)
            {
                return Result<bool>.ValidationError(
                    $"Payroll has already been processed for {reqModel.PayrollMonth}.");
            }

            #endregion

            var result = await _daPayroll.ProcessPayroll(reqModel);
            return result;
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
            return Result<bool>.SystemError("Error occured while processing payroll!");
        }
    }

    private Result<bool> ValidatePayrollMonth(string payrollMonth)
    {
        if (string.IsNullOrWhiteSpace(payrollMonth))
            return Result<bool>.ValidationError("PayrollMonth is required.");

        // Regex: 3-letter month + space + 4-digit year
        var regex = new Regex(@"^(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\s\d{4}$",
            RegexOptions.IgnoreCase);

        if (!regex.IsMatch(payrollMonth))
            return Result<bool>.ValidationError("PayrollMonth must be in the format 'MMM yyyy' (e.g., Nov 2025).");

        return Result<bool>.Success();
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

    public async Task<Result<MonthlyPayrollChartResponseModel>> EmployeePayrollChart(
        MonthlyPayrollChartRequestModel reqModel)
    {
        try
        {
            var result = await _daPayroll.GetMonthlyPayrollChartAsync(reqModel);
            return result;
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString(), "Error fetching payroll list.");
            return Result<MonthlyPayrollChartResponseModel>.SystemError("Error fetching payroll list!");
        }
    }

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