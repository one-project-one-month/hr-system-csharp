using HRSystem.Csharp.Domain.Features.Employee;
using HRSystem.Csharp.Domain.Models.Employee;

namespace HRSystem.Csharp.Api.Controllers;

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
    public async Task<IActionResult> GetAllEmployee()
    {
        return Ok(await _blEmployee.GetAllEmployee());
    }

    [HttpGet("EditEmployee/{employeeCode}")]
    public async Task<IActionResult> EditEmployee(string employeeCode)
    {
       return Ok(await _blEmployee.EditEmployee(employeeCode));
    }

    [HttpPost("CreateEmployee")]
    public async Task<IActionResult> CreateEmployee([FromBody] EmployeeCreateRequestModel req)
    {
     
        return Ok(await _blEmployee.CreateEmployee(req));
    }

    [HttpPost("UpdateEmployee/{employeeCode}")]
    public async Task<IActionResult> UpdateEmployee(string employeeCode, [FromBody] EmployeeUpdateRequestModel req)
    {
       return Ok(await _blEmployee.UpdateEmployee(employeeCode, req));
    }

    [HttpPost("DeleteEmployee/{employeeCode}")]
    public async Task<IActionResult> DeleteEmployee(string employeeCode)
    {
       return Ok(await _blEmployee.DeleteEmployee(employeeCode));
    }
}