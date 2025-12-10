using HRSystem.Csharp.Domain.Features.Payroll;
using HRSystem.Csharp.Domain.Models.Payroll;
using Microsoft.AspNetCore.Authorization;

namespace HRSystem.Csharp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PayrollController : ControllerBase
{
    private readonly BL_Payroll _blPayroll;
    private readonly ILogger<PayrollController> _logger;

    public PayrollController(BL_Payroll blPayroll, ILogger<PayrollController> logger)
    {
        _blPayroll = blPayroll;
        _logger = logger;
    }

    [HttpPost("process")]
    public async Task<IActionResult> ProcessPayroll(PayrollProcessRequestModel reqModel)
    {
        try
        {
            var result = await _blPayroll.ProcessPayroll(reqModel);
            if (result.IsError)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
            return StatusCode(500, "Unexpected error occurred.");
        }
    }

    [HttpGet("summary-list")]
    public async Task<IActionResult> PayrollSummaryList([FromQuery] PayrollListRequestModel reqModel)
    {
        var result = await _blPayroll.PayrollList(reqModel);
        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpGet("month-detail-list")]
    public async Task<IActionResult> PayrollMonthDetailList([FromQuery] PayrollMonthDetailListRequestModel reqModel)
    {
        var result = await _blPayroll.PayrollMonthDetailList(reqModel);
        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
    
    [HttpGet("list/employee")]
    public async Task<IActionResult> EmployeePayrollList([FromQuery] EmployeePayrollListRequestModel reqModel)
    {
        var result = await _blPayroll.EmployeePayrollList(reqModel);
        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    /*[HttpGet("employee-chart")]
    public async Task<IActionResult> EmployeePayrollChart([FromQuery] EmployeePayrollListRequestModel reqModel)
    {
        var result = await _blPayroll.EmployeePayrollChart(reqModel);
        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }*/

    /*[HttpGet("list")]
    public async Task<IActionResult> GetPayrollList([FromQuery] PayrollRequestModel model)
    {
        try
        {
            var result = await _blPayroll.GetPayrollList(model);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            _logger.LogError($"Error Occured while fetching payroll list: {ex.Message}");
            return Problem(
                detail: $"An unexpected error occurred while fetching payroll list.",
                title: "Internal Server Error",
                statusCode: 500
            );
        }
    }*/
}