using HRSystem.Csharp.Domain.Features.Payroll;
using HRSystem.Csharp.Domain.Models.Payroll;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRSystem.Csharp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayrollController : ControllerBase
    {
        private readonly BL_Payroll _blPayroll;
        private readonly ILogger<PayrollController> _logger;

        public PayrollController (BL_Payroll blPayroll, ILogger<PayrollController> logger)
        {
            _blPayroll = blPayroll;
            _logger = logger;

        }
        [HttpGet("list")]
        public async Task<IActionResult> GetPayrollList([FromQuery]PayrollRequestModel model)
        {
            try
            {
                var result = await _blPayroll.GetPayrollList(model);
                return Ok(result);
            } catch (Exception ex) {
                _logger.LogError(ex.ToString());
                _logger.LogError($"Error Occured while fetching payroll list: {ex.Message}");
                return Problem(
                    detail: $"An unexpected error occurred while fetching payroll list.",
                    title: "Internal Server Error",
                    statusCode: 500
                );
            }
        }
        

    }
}
