using HRSystem.Csharp.Domain.Models.Employee;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;

namespace HRSystem.Csharp.Domain.Features.Employee
{
    public class BL_Employee
    {
        private readonly DA_Employee _daEmployee;

        public BL_Employee(DA_Employee daEmployee)
        {
            _daEmployee = daEmployee;
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