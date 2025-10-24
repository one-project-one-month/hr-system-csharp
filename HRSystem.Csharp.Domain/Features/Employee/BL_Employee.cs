using HRSystem.Csharp.Domain.Models.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public Result<List<EmployeeResponseModel>> GetAllEmployee()
        {
            var employees = _daEmployee.GetAllEmployee();
            var response = Result<List<EmployeeResponseModel>>.Success(employees.Data);
            return response;
        }

        public Result<List<EmployeeEditResponseModel>> EditEmployee(string employeeCode)
        {
            var employees = _daEmployee.EditEmployee(employeeCode);
            var response = Result<List<EmployeeEditResponseModel>>.Success(employees.Data);
            return response;
        }

        public Result<EmployeeCreateResponseModel> CreateEmployee(EmployeeCreateRequestModel req)
        {
            var result = _daEmployee.CreateEmployee(req);
            return result;
        }

        public Result<EmployeeUpdateResponseModel> UpdateEmployee(string employeeCode, EmployeeUpdateRequestModel req)
        {
            var result = _daEmployee.UpdateEmployee(employeeCode, req);
            return result;
        }

        public Result<EmployeeDeleteResponseModel> DeleteEmployee(string employeeCode)
        {
            var result = _daEmployee.DeleteEmployee(employeeCode);
            return result;
        }
    }
}
