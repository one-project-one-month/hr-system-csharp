using HRSystem.Csharp.Domain.Features.Employee;
using HRSystem.Csharp.Domain.Models.Employee;
using Microsoft.AspNetCore.Authorization;

namespace HRSystem.Csharp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class EmployeeController : ControllerBase
{
    private readonly BL_Employee _blEmployee;

    public EmployeeController(BL_Employee blEmployee)
    {
        _blEmployee = blEmployee;
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetAllEmployee([FromQuery] EmployeeListRequestModel reqModel)
    {
        var result = await _blEmployee.GetAllEmployee(reqModel);
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }

        return BadRequest(result);
    }
    
    [HttpGet("emp-role-list")]
    public async Task<IActionResult> GetEmployeeUserList([FromQuery] EmployeeListRequestModel reqModel)
    {
        var result = await _blEmployee.GetEmployeeUserList(reqModel);
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }

        return BadRequest(result);
    }

    [HttpGet("edit/{employeeCode}")]
    public async Task<IActionResult> EditEmployee(string employeeCode)
    {
        var result = await _blEmployee.EditEmployee(employeeCode);
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }

        return BadRequest(result);
    }

    [HttpGet("profile/{employeeCode}")]
    public async Task<IActionResult> GetUserProfile(string employeeCode)
    {
        var result = await _blEmployee.GetUserProfile(employeeCode);
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }

        return BadRequest(result);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateEmployee([FromBody] EmployeeCreateRequestModel req)
    {
        var result = await _blEmployee.CreateEmployee(req);
        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPut("update/{employeeCode}")]
    public async Task<IActionResult> UpdateEmployee(string employeeCode, [FromBody] EmployeeUpdateRequestModel req)
    {
        var result = await _blEmployee.UpdateEmployee(employeeCode, req);
        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpDelete("delete/{employeeCode}")]
    public async Task<IActionResult> DeleteEmployee(string employeeCode)
    {
        var result = await _blEmployee.DeleteEmployee(employeeCode);
        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("EditProfile")]
    public async Task<IActionResult> EditProfile([FromForm] EmployeeEditProfileRequestModel requestModel)
    {   
        var result = await _blEmployee.EditProfile(requestModel);
        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
}