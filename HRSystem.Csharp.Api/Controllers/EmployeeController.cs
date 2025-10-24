using HRSystem.Csharp.Domain.Features.Employee;
using HRSystem.Csharp.Domain.Models.Employee;

namespace HRSystem.Csharp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        
        private readonly BL_Employee _blEmployee;
        public EmployeeController(BL_Employee blEmployee)
        {
            _blEmployee = blEmployee;
        }
        [HttpGet("GetAllEmployee")]
        public IActionResult GetAllEmployee()
        {
            var result = _blEmployee.GetAllEmployee();
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest(result);
        }

        [HttpGet("EditEmployee/{employeeCode}")]
        public IActionResult EditEmployee(string employeeCode) {
            var result = _blEmployee.EditEmployee(employeeCode);
            if (result.IsSuccess) {
                return Ok(result.Data);
            }
            return BadRequest(result);
        }

        [HttpPost("CreateEmployee")]
        public IActionResult CreateEmployee([FromBody] EmployeeCreateRequestModel req)
        {
            var result = _blEmployee.CreateEmployee(req);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            return BadRequest(result);
        }

        [HttpPost("UpdateEmployee/{employeeCode}")]
        public IActionResult UpdateEmployee(string employeeCode, [FromBody] EmployeeUpdateRequestModel req)
        {
            var result = _blEmployee.UpdateEmployee(employeeCode, req);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            return BadRequest(result);
        }

        [HttpPost("DeleteEmployee/{employeeCode}")]
        public IActionResult DeleteEmployee(string employeeCode)
        {
            var result = _blEmployee.DeleteEmployee(employeeCode);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            return BadRequest(result);
        }
    }
}
