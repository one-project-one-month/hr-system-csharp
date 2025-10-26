using HRSystem.Csharp.Database.AppDbContextModels;
using HRSystem.Csharp.Domain.Models.PayRoll;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HRSystem.Csharp.Domain.Features;

namespace HRSystem.Csharp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayrollController : ControllerBase
    {

        private readonly BL_Payroll _blPayroll;

        public PayrollController(BL_Payroll blPayroll)
        {
            _blPayroll = blPayroll;
        }

        [HttpGet("GetAllPayroll")]
        public IActionResult GetAllPayroll()
        {
            var result = _blPayroll.GetAllPayroll();
            if (result.IsSuccess)
                return Ok(result.Data);

            return BadRequest(result);
        }

        [HttpGet("EditPayroll/{payrollCode}")]
        public IActionResult EditPayroll(string payrollCode)
        {
            var result = _blPayroll.EditPayroll(payrollCode);
            if (result.IsSuccess)
                return Ok(result.Data);

            return BadRequest(result);
        }

        [HttpPost("CreatePayroll")]
        public IActionResult CreatePayroll([FromBody] PayrollCreateRequestModel req)
        {
            var result = _blPayroll.CreatePayroll(req);
            if (result.IsSuccess)
                return Ok(result.Message);

            return BadRequest(result);
        }

        [HttpPut("UpdatePayroll/{payrollCode}")]
        public IActionResult UpdatePayroll(string payrollCode, [FromBody] PayrollUpdateRequestModel req)
        {
            var result = _blPayroll.UpdatePayroll(payrollCode, req);
            if (result.IsSuccess)
                return Ok(result.Message);

            return BadRequest(result);
        }

        [HttpDelete("DeletePayroll/{payrollCode}")]
        public IActionResult DeletePayroll(string payrollCode)
        {
            var result = _blPayroll.DeletePayroll(payrollCode);
            if (result.IsSuccess)
                return Ok(result.Message);

            return BadRequest(result);
        }
    }
}
