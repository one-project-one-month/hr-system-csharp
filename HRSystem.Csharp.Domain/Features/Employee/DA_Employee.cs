using HRSystem.Csharp.Domain.Models.Employee;
using System.Data;
using Microsoft.Extensions.Logging;

namespace HRSystem.Csharp.Domain.Features.Employee;

public class DA_Employee
{
    string currentUser = "Admin"; //for testing only

    private readonly AppDbContext _appDbContext;
    private readonly ILogger<DA_Employee> _logger;

    public async Task<Result<List<EmployeeResponseModel>>> GetAllEmployee()
    {
        try
        {
            var result = await
                (from e in _appDbContext.TblEmployees
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
                          }).AsNoTracking().ToListAsync();
            return Result<List<EmployeeResponseModel>>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<List<EmployeeResponseModel>>.Error($"An error occurred while retrieving employees: {ex.Message}");
        }

    }
    public async Task<Result<List<EmployeeEditResponseModel>>> EditEmployee(string employeeCode)
    {
        try
        {
            var result = await _appDbContext.TblEmployees.Where(e => e.EmployeeCode == employeeCode && e.DeleteFlag != true)
                .Select(e => new EmployeeEditResponseModel
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
                }).AsNoTracking().ToListAsync();
            if (result == null)
            {
                return Result<List<EmployeeEditResponseModel>>.ValidationError("EmployeeCode Not Found");
            }
            return Result<List<EmployeeEditResponseModel>>.Success(result);

            if (!string.IsNullOrWhiteSpace(reqModel.EmployeeName))
            {
                query = query.Where(r => r.Name != null
                                         && r.Name.ToLower() == reqModel.EmployeeName.ToLower());
            }

    public async Task<Result<EmployeeCreateResponseModel>> CreateEmployee(EmployeeCreateRequestModel req)
    {
        if (duplicateUsername(req.Username))
        {
            return Result<EmployeeCreateResponseModel>.ValidationError("Username already exist");
        }
        if (duplicateEmail(req.Email))
        {
            return Result<EmployeeCreateResponseModel>.ValidationError("Email is duplicate");
        }
        if (duplicatePhoneNo(req.PhoneNo))
        {
            return Result<EmployeeCreateResponseModel>.ValidationError("Phone number already exist");
        }
        try
        {
                var existingRole = await _appDbContext.TblRoles.AsNoTracking().FirstOrDefaultAsync(r => r.RoleCode == req.RoleCode);
                if (existingRole == null)
                {
                    return Result<EmployeeCreateResponseModel>.ValidationError("RoleCode does not exist");
                }
                var newEmployee = new TblEmployee
            {
                EmployeeId = Ulid.NewUlid().ToString(),
                EmployeeCode = "EMP" + new Random().Next(1000, 9999).ToString(), //testing only
                RoleCode = req.RoleCode,
                Username = req.Username,
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

            await _appDbContext.TblEmployees.AddAsync(newEmployee);
            await _appDbContext.SaveChangesAsync();

                return Result<EmployeeCreateResponseModel>.Success(new EmployeeCreateResponseModel(), "Employee Created Successfully");
            }
            catch (Exception ex)
            {
                Items = pagedResult.Items,
                TotalCount = pagedResult.TotalCount,
                PageNo = reqModel.PageNo,
                PageSize = reqModel.PageSize
            };

            return Result<EmployeeListResponseModel>.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching employees");
            return Result<EmployeeListResponseModel>.SystemError("An error occurred while retrieving employees.");
        }
    }


    public async Task<Result<EmployeeUpdateResponseModel>> UpdateEmployee(string empCode, EmployeeUpdateRequestModel emp)
    {
        try
        {
            var existingEmp = await _appDbContext.TblEmployees.AsNoTracking().FirstOrDefaultAsync(e => e.EmployeeCode == empCode && e.DeleteFlag != true);
            if (existingEmp == null) return Result<EmployeeUpdateResponseModel>.NotFoundError("Cannot find the employee to be updated");
                var existingRole = await _appDbContext.TblRoles.AsNoTracking().FirstOrDefaultAsync(r => r.RoleCode == emp.RoleCode);
                if (existingRole == null)
                {
                    return Result<EmployeeUpdateResponseModel>.ValidationError("RoleCode does not exist");
                }
                if (duplicateUpdateEmail(emp.Email, empCode))
            {
                return Result<EmployeeUpdateResponseModel>.ValidationError("Email is duplicate");
            }
            if (duplicateUpdatePhoneNo(emp.PhoneNo, empCode))
            {
                return Result<EmployeeUpdateResponseModel>.ValidationError("Phone number already exist");
            }

            existingEmp.Name = emp.Name;
            existingEmp.RoleCode = emp.RoleCode;
            existingEmp.Email = emp.Email;
            existingEmp.PhoneNo = emp.PhoneNo;
            existingEmp.Salary = emp.Salary;
            existingEmp.StartDate = emp.Startdate;
            existingEmp.ResignDate = emp.Resigndate;
            existingEmp.ModifiedAt = DateTime.UtcNow;
            existingEmp.ModifiedBy = currentUser;
            _appDbContext.SaveChangesAsync();
            return Result<EmployeeUpdateResponseModel>.Success(new EmployeeUpdateResponseModel(), "Employee Updated Successfully");
        }
        catch (Exception ex) {
            return Result<EmployeeUpdateResponseModel>.SystemError(ex.Message);
        }
    }

    public async Task<Result<EmployeeDeleteResponseModel>> DeleteEmployee(string employeeCode)
    {
        try
        {
            var model = await _appDbContext.TblEmployees.AsNoTracking().FirstOrDefaultAsync(e => e.EmployeeCode == employeeCode &&
                       e.DeleteFlag == false);
            if (model == null) return Result<EmployeeDeleteResponseModel>.NotFoundError("Cannot find the employee code");

            model.DeleteFlag = true;
            model.ModifiedAt = DateTime.UtcNow;
            model.ModifiedBy = currentUser;
            _appDbContext.SaveChangesAsync();
            return Result<EmployeeDeleteResponseModel>.Success(new EmployeeDeleteResponseModel(), "Employee Deleted Successfully");

        }
        catch (Exception ex)
        {
            return Result<EmployeeDeleteResponseModel>.SystemError($"An error occurred while deleting employee: {ex.Message}");
        }
    }

    #region Helper function

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

        return null;
    }*/

    /*private Result<EmployeeUpdateResponseModel> UpdateValidation(EmployeeUpdateRequestModel req, string empCode)
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
            return Result<EmployeeUpdateResponseModel>.ValidationError(
                "Phone number cannot be empty or less than 9 numbers!");
        }

        if (duplicateUpdatePhoneNo(req.PhoneNo, empCode))
        {
            return Result<EmployeeUpdateResponseModel>.ValidationError("Phone number already exist");
        }

        return null;
    }*/
}