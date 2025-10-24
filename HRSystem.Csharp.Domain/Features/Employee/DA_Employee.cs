using HRSystem.Csharp.Domain.Models.Employee;
using HRSystem.Csharp.Domain.Models.Roles;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Csharp.Domain.Features.Employee
{
    public class DA_Employee
    {
        string currentUser = "Admin"; //for testing only

        public readonly AppDbContext _appDbContext;
        public DA_Employee(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public Result<List<EmployeeResponseModel>> GetAllEmployee()
        {
            try
            {
              var result = (from e in _appDbContext.TblEmployees
                           join r in _appDbContext.TblRoles on e.RoleCode equals r.RoleCode
                           where e.DeleteFlag != true
                           select new EmployeeResponseModel
                           {        
                               EmployeeCode = e.EmployeeCode,
                               ProfileImage = e.ProfileImage,
                               Username = e.Username,
                               Name = e.Name,
                               RoleName = r.RoleName, 
                               Email = e.Email,
                               PhoneNo = e.PhoneNo
                           }).ToList();
                return Result<List<EmployeeResponseModel>>.Success(result);
            }
            catch (Exception ex)
            {
                return Result<List<EmployeeResponseModel>>.Error($"An error occurred while retrieving employees: {ex.Message}");
            }
        }
        public Result<List<EmployeeEditResponseModel>> EditEmployee(string employeeCode)
        {
            try
            {
                var result = _appDbContext.TblEmployees.Where(e=>e.EmployeeCode == employeeCode && e.DeleteFlag != true)
                    .Select(e=> new EmployeeEditResponseModel
                    {
                        EmployeeCode = e.EmployeeCode,
                        Username = e.Username,
                        Name = e.Name,
                        RoleCode = e.RoleCode,
                        Email = e.Email,
                        PhoneNo = e.PhoneNo,
                        Salary = e.Salary,
                        StartDate = e.StartDate,
                        ResignDate = e.ResignDate
                    }).ToList();
                if(result == null)
                {
                    return Result<List<EmployeeEditResponseModel>>.ValidationError("EmployeeCode Not Found");
                }
                return Result<List<EmployeeEditResponseModel>>.Success(result);

            }
            catch (Exception ex)
            {
                return Result<List<EmployeeEditResponseModel>>.Error($"An error occurred while retrieving employees: {ex.Message}");
            }
        }

        public Result<EmployeeCreateResponseModel> CreateEmployee(EmployeeCreateRequestModel req)
        {
            try
            {

                var validationResult = CreateValiadation(req);
                if (validationResult != null)
                {
                    return validationResult;
                }
                var newEmployee = new TblEmployee
                {
                    EmployeeId = Ulid.NewUlid().ToString(),
                    EmployeeCode = "EMP" + new Random().Next(1000, 9999).ToString(), //testing only
                    RoleCode = req.RoleCode,
                    Username =  req.Username,
                    Name = req.Name,
                    Email = req.Email,
                    PhoneNo = req.PhoneNo,
                    Password = req.Password,
                    IsFirstTimeLogin = true,
                    ProfileImage = null,
                    Salary = req.Salary,
                    StartDate = req.StartDate,
                    ResignDate = req.ResignDate,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = currentUser, //testing only
                    DeleteFlag = false

                };

                _appDbContext.TblEmployees.Add(newEmployee);
                _appDbContext.SaveChanges();

                return Result<EmployeeCreateResponseModel>.Success(new EmployeeCreateResponseModel(), "Employee Created Successfully");
            }
            catch (Exception ex)
            {
                return Result<EmployeeCreateResponseModel>.Error($"An error occurred while creating employee: {ex.Message}");
            }
        }

        public Result<EmployeeUpdateResponseModel> UpdateEmployee(string empCode, EmployeeUpdateRequestModel emp)
        {
            var existingEmp = _appDbContext.TblEmployees.FirstOrDefault(e => e.EmployeeCode == empCode && e.DeleteFlag != true);
            if (existingEmp == null) return Result<EmployeeUpdateResponseModel>.NotFoundError("Cannot find the role to be updated");

            var validationResult = UpdateValiadation(emp, empCode);
            if (validationResult != null)
            {
                return validationResult;
            }
            existingEmp.Name = emp.Name;
            existingEmp.RoleCode = emp.RoleCode;
            existingEmp.Email = emp.Email;
            existingEmp.PhoneNo = emp.PhoneNo;
            existingEmp.Salary = emp.Salary;
            existingEmp.StartDate = emp.Startdate;
            existingEmp.ResignDate = emp.Resigndate;
            existingEmp.ModifiedAt = DateTime.Now;
            existingEmp.ModifiedBy = currentUser;
            _appDbContext.SaveChanges();
            return Result<EmployeeUpdateResponseModel>.Success(new EmployeeUpdateResponseModel(), "Employee Updated Successfully");

        }

        public Result<EmployeeDeleteResponseModel> DeleteEmployee(string employeeCode)
        {
            var model = _appDbContext.TblEmployees.FirstOrDefault(e => e.EmployeeCode == employeeCode &&
                             e.DeleteFlag == false);
            if (model == null) return Result<EmployeeDeleteResponseModel>.NotFoundError("Cannot find the employee code");

            model.DeleteFlag = true;
            model.ModifiedAt = DateTime.Now;
            model.ModifiedBy = currentUser;
            _appDbContext.SaveChanges();
            return Result<EmployeeDeleteResponseModel>.Success(new EmployeeDeleteResponseModel(), "Employee Deleted Successfully");
                  

        }

      


        #region Helper function
        private Result<EmployeeCreateResponseModel> CreateValiadation(EmployeeCreateRequestModel req)
        {
            if(req.Username == null || req.Username.Trim() == "")
            {
                return Result<EmployeeCreateResponseModel>.ValidationError("Username is required");
            }

            if (duplicateUsername(req.Username))
            {
                return Result<EmployeeCreateResponseModel>.ValidationError("Username already exist");
            }

            if (req.Name == null || req.Name.Trim() == "")
            {
                return Result<EmployeeCreateResponseModel>.ValidationError("Name is required");
            }
            if (req.Password == null || req.Password.Trim() == "" || req.Password!.Length < 8)
            {
                return Result<EmployeeCreateResponseModel>.ValidationError("Password is required and more than 8 charactar");
            }
            if (req.Email == null || req.Email.Trim() == "")
            {
                return Result<EmployeeCreateResponseModel>.ValidationError("Email is required");
            }

            if (!checkEmail(req.Email))
            {
                return Result<EmployeeCreateResponseModel>.ValidationError("Email format is not valid");
            }

            if (duplicateEmail(req.Email))
            {
                return Result<EmployeeCreateResponseModel>.ValidationError("Email is duplicate");
            }

            if (req.PhoneNo == null || req.PhoneNo.Trim() == "" || req.PhoneNo!.Length < 9)
            {
                return Result<EmployeeCreateResponseModel>.ValidationError("Phone number cannot be empty or less than 9 numbers!");
            }

            if (duplicatePhoneNo(req.PhoneNo))
            {
                return Result<EmployeeCreateResponseModel>.ValidationError("Phone number already exist");
            }

          
            return null;
        }


        private Result<EmployeeUpdateResponseModel> UpdateValiadation(EmployeeUpdateRequestModel req, string empCode)
        {

            if (req.Name == null || req.Name.Trim() == "")
            {
                return Result<EmployeeUpdateResponseModel>.ValidationError("Name is required");
            }
       
            if (req.Email == null || req.Email.Trim() == "")
            {
                return Result<EmployeeUpdateResponseModel>.ValidationError("Email is required");
            }

            if (!checkEmail(req.Email))
            {
                return Result<EmployeeUpdateResponseModel>.ValidationError("Email format is not valid");
            }

            if (duplicateUpdateEmail(req.Email, empCode))
            {
                return Result<EmployeeUpdateResponseModel>.ValidationError("Email is duplicate");
            }

            if (req.PhoneNo == null || req.PhoneNo.Trim() == "" || req.PhoneNo!.Length < 9)
            {
                return Result<EmployeeUpdateResponseModel>.ValidationError("Phone number cannot be empty or less than 9 numbers!");
            }

            if (duplicateUpdatePhoneNo(req.PhoneNo, empCode))
            {
                return Result<EmployeeUpdateResponseModel>.ValidationError("Phone number already exist");
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
        public bool duplicateUsername(string username)
        {
            var isDuplicated = _appDbContext.TblEmployees.FirstOrDefault(x => x.Username == username);
            if (isDuplicated is null) return false;
            return true;
        }
        public bool duplicateEmail(string email)
        {
            var isDuplicated = _appDbContext.TblEmployees.FirstOrDefault(x => x.Email == email);
            if (isDuplicated is null) return false;
            return true;
        }

        public bool duplicatePhoneNo(string phoneNo)
        {
            var isDuplicated = _appDbContext.TblEmployees.FirstOrDefault(x => x.PhoneNo == phoneNo);
            if (isDuplicated is null) return false;
            return true;
        }


        public bool duplicateUpdateEmail(string email, string employeeCode)
        {
            var isDuplicated = _appDbContext.TblEmployees.FirstOrDefault(x => x.Email == email);
            if (isDuplicated is null)
            {
                return false;
            }
            else
            {
                if (isDuplicated.EmployeeCode == employeeCode) return false;
            }
          
            return true;
        }

        public bool duplicateUpdatePhoneNo(string phoneNo, string employeeCode)
        {
            var isDuplicated = _appDbContext.TblEmployees.FirstOrDefault(x => x.PhoneNo == phoneNo);
            if (isDuplicated is null)
            {
                return false;
            }
            else
            {
                if (isDuplicated.EmployeeCode == employeeCode) return false;
            }

            return true;
        }
        #endregion

      
    }
}
