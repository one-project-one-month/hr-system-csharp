using HRSystem.Csharp.Domain.Models.Employee;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;

namespace HRSystem.Csharp.Domain.Features.Employee
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

        public async Task<Result<List<EmployeeResponseModel>>> GetAllEmployee()
        {
            return await _daEmployee.GetAllEmployee();
        }

        public async Task<Result<List<EmployeeEditResponseModel>>> EditEmployee(string employeeCode)
        {
            return await _daEmployee.EditEmployee(employeeCode);
        }

        public async Task<Result<EmployeeCreateResponseModel>> CreateEmployee(EmployeeCreateRequestModel req)
        {
            var validationResult = CreateValiadation(req);
            if (validationResult != null)
            {
                return validationResult;
            }
            return await _daEmployee.CreateEmployee(req);
        }

        public async Task<Result<EmployeeUpdateResponseModel>> UpdateEmployee(string employeeCode, EmployeeUpdateRequestModel req)
        {
            var validationResult = UpdateValiadation(req);
            if (validationResult != null)
            {
                return validationResult;
            }

            return await _daEmployee.UpdateEmployee(employeeCode, req);
        }

        public async Task<Result<EmployeeDeleteResponseModel>> DeleteEmployee(string employeeCode)
        {
            return await _daEmployee.DeleteEmployee(employeeCode);
        }

        private Result<EmployeeCreateResponseModel> CreateValiadation(EmployeeCreateRequestModel req)
        {
            if (req.Username.IsNullOrEmpty())
            {
                return Result<EmployeeCreateResponseModel>.ValidationError("Username is required");
            }

            if (req.Name.IsNullOrEmpty())
            {
                return Result<EmployeeCreateResponseModel>.ValidationError("Name is required");
            }
            if (req.Password.IsNullOrEmpty())
            {
                return Result<EmployeeCreateResponseModel>.ValidationError("Password is required and more than 8 charactar");
            }
            if (req.Email.IsNullOrEmpty())
            {
                return Result<EmployeeCreateResponseModel>.ValidationError("Email is required");
            }

            if (!checkEmail(req.Email))
            {
                return Result<EmployeeCreateResponseModel>.ValidationError("Email format is not valid");
            }

            if (req.PhoneNo == null || req.PhoneNo.Trim() == "" || req.PhoneNo!.Length < 9)
            {
                return Result<EmployeeCreateResponseModel>.ValidationError("Phone number cannot be empty or less than 9 numbers!");
            }

            return null;
        }

        private Result<EmployeeUpdateResponseModel> UpdateValiadation(EmployeeUpdateRequestModel req)
        {

            if (req.Name.IsNullOrEmpty())
            {
                return Result<EmployeeUpdateResponseModel>.ValidationError("Name is required");
            }

            if (req.Email.IsNullOrEmpty())
            {
                return Result<EmployeeUpdateResponseModel>.ValidationError("Email is required");
            }

            if (!checkEmail(req.Email))
            {
                return Result<EmployeeUpdateResponseModel>.ValidationError("Email format is not valid");
            }

            if (req.PhoneNo == null || req.PhoneNo.Trim() == "" || req.PhoneNo!.Length < 9)
            {
                return Result<EmployeeUpdateResponseModel>.ValidationError("Phone number cannot be empty or less than 9 numbers!");
            }

            return null;
        }

        private static bool checkEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }




    }
}