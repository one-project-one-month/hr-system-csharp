using System.Net.Mail;
using HRSystem.Csharp.Domain.Models.Employee;

namespace HRSystem.Csharp.Domain.Features.Employee;

public class BL_Employee
{
    private readonly DA_Employee _daEmployee;

    public BL_Employee(DA_Employee daEmployee)
    {
        _daEmployee = daEmployee;
    }

    public async Task<Result<EmployeeListResponseModel>> GetAllEmployee(EmployeeListRequestModel reqModel)
    {
        
        var employees = await _daEmployee.GetAllEmployee(reqModel);
        return employees;
    }

    public async Task<Result<EmployeeEditResponseModel>> EditEmployee(string employeeCode)
    {
        var employees = await _daEmployee.GetEmployeeByCode(employeeCode);
        return employees;
    }

    public async Task<Result<UserProfileResponseModel>> getUserProfile(string employeeCode)
    {
        var result = await _daEmployee.GetUserProfile(employeeCode);
        return result;
    }

    public async Task<Result<EmployeeCreateResponseModel>> CreateEmployee(EmployeeCreateRequestModel reqModel)
    {
        #region User Input Validation

        if (string.IsNullOrWhiteSpace(reqModel.Username))
        {
            return Result<EmployeeCreateResponseModel>.ValidationError("UserName is required!");
        }

        if (string.IsNullOrWhiteSpace(reqModel.Name))
        {
            return Result<EmployeeCreateResponseModel>.ValidationError("Name is required!");
        }

        if (string.IsNullOrWhiteSpace(reqModel.Email))
        {
            return Result<EmployeeCreateResponseModel>.ValidationError("Email is required!");
        }

        if (string.IsNullOrWhiteSpace(reqModel.PhoneNo))
        {
            return Result<EmployeeCreateResponseModel>.ValidationError("PhoneNo is required!");
        }

        if (reqModel.Salary <= 0)
        {
            return Result<EmployeeCreateResponseModel>.ValidationError("Salary is required!");
        }

        if (string.IsNullOrWhiteSpace(reqModel.RoleCode))
        {
            return Result<EmployeeCreateResponseModel>.ValidationError("Role is required!");
        }

        #endregion

        #region Duplicate Data Validation

        var userNameExist = await _daEmployee.GetEmployeeByUserName(reqModel.Username);
        if (userNameExist.IsSuccess)
        {
            return Result<EmployeeCreateResponseModel>.DuplicateRecordError("UserName already exists!");
        }

        var nameExist = await _daEmployee.GetEmployeeByName(reqModel.Name);
        if (nameExist.IsSuccess)
        {
            return Result<EmployeeCreateResponseModel>.DuplicateRecordError("Name already exists!");
        }

        var emailExist = await _daEmployee.DuplicateEmail(reqModel.Email);
        if (emailExist.IsSuccess)
        {
            return Result<EmployeeCreateResponseModel>.DuplicateRecordError("Email already exists!");
        }

        var validEmail = new MailAddress(reqModel.Email);
        if (validEmail.Address != reqModel.Email)
        {
            return Result<EmployeeCreateResponseModel>.BadRequestError("Invalid email format.");
        }


        var phoneExist = await _daEmployee.DuplicatePhoneNo(reqModel.PhoneNo);
        if (phoneExist.IsSuccess)
        {
            return Result<EmployeeCreateResponseModel>.DuplicateRecordError("PhoneNo already exists!");
        }


        var roleExist = await _daEmployee.roleCodeExist(reqModel.RoleCode);
        if (!roleExist.IsSuccess)
        {
            return Result<EmployeeCreateResponseModel>.NotFoundError("Role Code not found.");
        }
        #endregion

        var result = await _daEmployee.CreateEmployee(reqModel);
        return result;
    }

    public async Task<Result<EmployeeUpdateResponseModel>> UpdateEmployee(string employeeCode,
        EmployeeUpdateRequestModel reqModel)
    {
        #region User Input Validation

        if (string.IsNullOrWhiteSpace(reqModel.Name))
        {
            return Result<EmployeeUpdateResponseModel>.ValidationError("Name is required!");
        }

        if (string.IsNullOrWhiteSpace(reqModel.Email))
        {
            return Result<EmployeeUpdateResponseModel>.ValidationError("Email is required!");
        }

        if (string.IsNullOrWhiteSpace(reqModel.PhoneNo))
        {
            return Result<EmployeeUpdateResponseModel>.ValidationError("PhoneNo is required!");
        }

        if (reqModel.Salary <= 0)
        {
            return Result<EmployeeUpdateResponseModel>.ValidationError("Salary is required!");
        }

        if (string.IsNullOrWhiteSpace(reqModel.RoleCode))
        {
            return Result<EmployeeUpdateResponseModel>.ValidationError("Role is required!");
        }

        #endregion

        #region Check Employee Exists

        var employee = await _daEmployee.GetEmployeeByCode(employeeCode);
        if (employee?.Data == null)
        {
            return Result<EmployeeUpdateResponseModel>.NotFoundError("Employee doesn't exist!");
        }

        #endregion


        #region Check Role Code Exists  
        var roleExist = await _daEmployee.roleCodeExist(reqModel.RoleCode);
        if (!roleExist.IsSuccess)
        {
            return Result<EmployeeUpdateResponseModel>.NotFoundError("Role Code not found.");
        }
        #endregion

        #region Duplicate Data Validation

        var emailExist = await _daEmployee.DuplicateUpdateEmail(employeeCode, reqModel.Email);
        if (emailExist.IsSuccess)
        {
            return Result<EmployeeUpdateResponseModel>.DuplicateRecordError("Email already exists!");
        }

        try
        {
            var validEmail = new MailAddress(reqModel.Email);
        }
        catch
        {
            return Result<EmployeeUpdateResponseModel>.BadRequestError("Invalid email format.");
        }


        var phoneExist = await _daEmployee.DuplicateUpdatePhoneNo(employeeCode, reqModel.PhoneNo);
        if (phoneExist.IsSuccess)
        {
            return Result<EmployeeUpdateResponseModel>.DuplicateRecordError("PhoneNo already exists!");
        }

        #endregion

        var result = await _daEmployee.UpdateEmployee(employeeCode, reqModel);
        return result;
    }

    public async Task<Result<EmployeeDeleteResponseModel>> DeleteEmployee(string employeeCode)
    {
        var result = await _daEmployee.DeleteEmployee(employeeCode);
        return result;
    }


}