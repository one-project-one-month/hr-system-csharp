using Azure.Core;
using HRSystem.Csharp.Domain.Models.Project;
using static Dapper.SqlMapper;

namespace HRSystem.Csharp.Domain.Features.Employee;

public class DA_Employee(
    IHttpContextAccessor httpContextAccessor,
    AppDbContext appDbContext,
    ILogger<DA_Employee> logger,
    DA_Sequence daSequence,
    JwtService jwtService) : AuthorizationService(httpContextAccessor)
{
    private readonly AppDbContext _appDbContext = appDbContext;
    private readonly ILogger<DA_Employee> _logger = logger;
    private readonly JwtService _jwtService = jwtService;
    private readonly DA_Sequence _daSequence = daSequence;

    public async Task<Result<EmployeeListResponseModel>> GetEmployeeList(EmployeeListRequestModel reqModel)
    {
        try
        {
            var query = _appDbContext.TblEmployees
                .AsNoTracking()
                .Where(e => !e.DeleteFlag)
                .Join(_appDbContext.TblRoles,
                    e => e.RoleCode,
                    r => r.RoleCode,
                    (e, r) => new EmployeeResponseModel
                    {
                        EmployeeCode = e.EmployeeCode,
                        ProfileImage = e.ProfileImage,
                        Username = e.Username,
                        Name = e.Name,
                        RoleName = r.RoleName,
                        Email = e.Email,
                        PhoneNo = e.PhoneNo,
                        CreatedAt = e.CreatedAt
                    });

            if (!string.IsNullOrWhiteSpace(reqModel.EmployeeName))
            {
                query = query.Where(r => r.Name != null
                                         && r.Name.ToLower().Contains(reqModel.EmployeeName.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(reqModel.RoleName))
            {
                query = query.Where(r => r.RoleName != null
                                         && r.RoleName.ToLower().Equals(reqModel.RoleName.ToLower()));
            }

            query = query.OrderByDescending(r => r.CreatedAt);

            var pagedResult = await query.GetPagedResultAsync(reqModel.PageNo, reqModel.PageSize);

            var result = new EmployeeListResponseModel
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
            return Result<EmployeeListResponseModel>.Error(
                $"An error occurred while retrieving employees: {ex.Message}");
        }
    }

    public async Task<Result<EmployeeEditResponseModel>> GetEmployeeByCode(string employeeCode)
    {
        try
        {
            var employee = await _appDbContext.TblEmployees
                .FirstOrDefaultAsync(e => e.EmployeeCode == employeeCode && e.DeleteFlag == false);

            if (employee == null)
            {
                return Result<EmployeeEditResponseModel>.ValidationError("Employee doesn't exist!");
            }

            var result = new EmployeeEditResponseModel()
            {
                EmployeeCode = employee.EmployeeCode,
                Username = employee.Username,
                Name = employee.Name,
                RoleCode = employee.RoleCode,
                Email = employee.Email,
                PhoneNo = employee.PhoneNo,
                Salary = employee.Salary,
                StartDate = employee.StartDate,
                ResignDate = employee.ResignDate,
                Gender = employee.Gender
            };
            return Result<EmployeeEditResponseModel>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<EmployeeEditResponseModel>.Error(
                $"An error occurred while retrieving employees: {ex.Message}");
        }
    }

    public async Task<Result<AddEmployeeToProjectResponseModel>> ValidateEmployeesExist(
        AddEmployeeToProjectRequestModel reqModel)
    {
        var existingEmployees = await _appDbContext.TblEmployees
            .AsNoTracking()
            .Where(e => reqModel.EmployeeCodes.Contains(e.EmployeeCode) && !e.DeleteFlag)
            .Select(e => e.EmployeeCode)
            .ToListAsync();

        var invalidEmployees = reqModel.EmployeeCodes.Except(existingEmployees).ToList();

        if (invalidEmployees.Count != 0)
        {
            return Result<AddEmployeeToProjectResponseModel>.ValidationError(
                $"Employees not found: {string.Join(", ", invalidEmployees)}",
                new AddEmployeeToProjectResponseModel
                {
                    EmployeeCodes = invalidEmployees
                }
            );
        }

        return Result<AddEmployeeToProjectResponseModel>.Success(new AddEmployeeToProjectResponseModel(),
            "All employees exist!");
    }

    public async Task<Result<UserProfileResponseModel>> GetUserProfile(string employeeCode)
    {
        try
        {
            var result = await _appDbContext.TblEmployees
                .AsNoTracking()
                .Where(e => e.EmployeeCode == employeeCode && e.DeleteFlag == false)
                .Join(_appDbContext.TblRoles,
                    e => e.RoleCode,
                    r => r.RoleCode,
                    (e, r) => new UserProfileResponseModel
                    {
                        EmployeeCode = e.EmployeeCode,
                        ProfileImage = e.ProfileImage,
                        Username = e.Username,
                        Name = e.Name,
                        RoleName = r.RoleName,
                        Email = e.Email,
                        PhoneNo = e.PhoneNo,
                        Gender = e.Gender
                    })
                .FirstOrDefaultAsync();

            if (result is null)
            {
                return Result<UserProfileResponseModel>.ValidationError("Employee doesn't exist!");
            }

            return Result<UserProfileResponseModel>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<UserProfileResponseModel>.Error(
                $"An error occurred while retrieving employees: {ex.Message}");
        }
    }

    public async Task<Result<EmployeeCreateResponseModel>> CreateEmployee(
        EmployeeCreateRequestModel reqModel)
    {
        try
        {
            var hashPassword = _jwtService.HashPassword(reqModel.Password);

            var generatedCode = await _daSequence.GenerateCodeAsync(EnumSequenceCode.EMP.ToString());
            var newEmployee = new TblEmployee
            {
                EmployeeId = DevCode.GenerateNewUlid(),
                EmployeeCode = generatedCode,
                RoleCode = reqModel.RoleCode,
                Username = reqModel.Username,
                Name = reqModel.Name,
                Email = reqModel.Email,
                PhoneNo = reqModel.PhoneNo,
                Password = hashPassword,
                IsFirstTimeLogin = true,
                ProfileImage = "Profile",
                Salary = reqModel.Salary,
                StartDate = reqModel.StartDate,
                ResignDate = reqModel.ResignDate,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = UserCode!,
                DeleteFlag = false
            };

            _appDbContext.TblEmployees.Add(newEmployee);

            Console.WriteLine($"ResignDate: {newEmployee.ResignDate}");

            await _appDbContext.TblEmployees.AddAsync(newEmployee);
            await _appDbContext.SaveChangesAsync();

            return Result<EmployeeCreateResponseModel>.Success("Employee Created Successfully");
        }
        catch (Exception ex)
        {
            return Result<EmployeeCreateResponseModel>.Error(
                $"An error occurred while creating employee: {ex.Message}");
        }
    }

    public async Task<Result<EmployeeUpdateResponseModel>> UpdateEmployee(string empCode,
        EmployeeUpdateRequestModel emp)
    {
        try
        {
            var existingEmp = await _appDbContext.TblEmployees
                .FirstOrDefaultAsync(e => e.EmployeeCode == empCode && e.DeleteFlag != true);

            if (existingEmp == null)
                return Result<EmployeeUpdateResponseModel>.NotFoundError("Cannot find the role to be updated");

            existingEmp.Name = emp.Name;
            existingEmp.RoleCode = emp.RoleCode;
            existingEmp.Email = emp.Email;
            existingEmp.PhoneNo = emp.PhoneNo;
            existingEmp.Salary = emp.Salary;
            existingEmp.StartDate = emp.StartDate;
            existingEmp.ResignDate = emp.ResignDate;
            existingEmp.ModifiedAt = DateTime.UtcNow;
<<<<<<< HEAD
<<<<<<< Updated upstream
            existingEmp.ModifiedBy = currentUser;
=======
            existingEmp.ModifiedBy = UserCode!;
            _appDbContext.TblEmployees.Update(existingEmp);
>>>>>>> Stashed changes
=======
            existingEmp.ModifiedBy = UserCode!;
>>>>>>> dev
            var updated = await _appDbContext.SaveChangesAsync() > 0;

            return updated
                ? Result<EmployeeUpdateResponseModel>.Success("Role updated successfully!")
                : Result<EmployeeUpdateResponseModel>.Error("Failed to update role.");
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            return Result<EmployeeUpdateResponseModel>.Error("Employee Update failed!");
        }
    }

    public async Task<Result<EmployeeDeleteResponseModel>> DeleteEmployee(string employeeCode)
    {
        var model = await _appDbContext.TblEmployees
            .FirstOrDefaultAsync(e => e.EmployeeCode == employeeCode && !e.DeleteFlag);

        if (model == null)
        {
            return Result<EmployeeDeleteResponseModel>.NotFoundError("Cannot find the employee code");
        }

        model.DeleteFlag = true;
        model.ModifiedAt = DateTime.UtcNow;
        model.ModifiedBy = UserCode!;

        await _appDbContext.SaveChangesAsync();

        return Result<EmployeeDeleteResponseModel>.Success(new EmployeeDeleteResponseModel(),
            "Employee Deleted Successfully");
    }

    public async Task<TblEmployee?> GetEmployeeByUserName(string username)
    {
        try
        {
            var employee = await _appDbContext.TblEmployees
                .FirstOrDefaultAsync(x => x.Username == username && !x.DeleteFlag);
            return employee;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching employee by name");
            throw new Exception("unexpected error occurred!");
        }
    }

    public async Task<Result<bool>> GetEmployeeByName(string fullName)
    {
        try
        {
            var employee = await _appDbContext.TblEmployees
                .FirstOrDefaultAsync(x => x.Name == fullName && !x.DeleteFlag);

            return employee != null
                ? Result<bool>.Success()
                : Result<bool>.NotFoundError("Employee not found.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching employee by name");
            return Result<bool>.SystemError("An error occurred while retrieving the employee.");
        }
    }

    public async Task<Result<bool>> DuplicateEmail(string email)
    {
        try
        {
            var emailExist = await _appDbContext.TblEmployees
                .FirstOrDefaultAsync(x => x.Email == email && !x.DeleteFlag);
            return emailExist != null
                ? Result<bool>.Success()
                : Result<bool>.NotFoundError("Email not found.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching employee by email");
            return Result<bool>.SystemError("An error occurred while retrieving the employee.");
        }
    }

    public async Task<Result<bool>> DuplicatePhoneNo(string phoneNo)
    {
        try
        {
            var phoneExist = await _appDbContext.TblEmployees
                .FirstOrDefaultAsync(x => x.PhoneNo == phoneNo && !x.DeleteFlag);

            return phoneExist != null
                ? Result<bool>.Success()
                : Result<bool>.NotFoundError("Phone No not found.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching employee by phone no");
            return Result<bool>.SystemError("An error occurred while retrieving the employee.");
        }
    }

    public async Task<Result<bool>> DuplicateUpdateEmail(string employeeCode, string email)
    {
        try
        {
            var emailExists = await _appDbContext.TblEmployees
                .AnyAsync(x =>
                    x.EmployeeCode != employeeCode &&
                    x.Email == email &&
                    !x.DeleteFlag);

            return emailExists
                ? Result<bool>.Success(true, "Email already exists for another employee.")
                : Result<bool>.NotFoundError("No duplicate email found.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking duplicate email");
            return Result<bool>.SystemError("An error occurred while checking email duplication.");
        }
    }

    public async Task<Result<bool>> DuplicateUpdateUserName(string employeeCode, string userName)
    {
        try
        {
            var userNameExists = await _appDbContext.TblEmployees
                .AnyAsync(x =>
                    x.EmployeeCode != employeeCode &&
                    x.Username == userName &&
                    !x.DeleteFlag);

            return userNameExists
                ? Result<bool>.Success(true, "UserName already exists for another employee.")
                : Result<bool>.NotFoundError("No duplicate username found.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking duplicate email");
            return Result<bool>.SystemError("An error occurred while checking email duplication.");
        }
    }

    public async Task<Result<bool>> DuplicateUpdatePhoneNo(string employeeCode, string phoneNo)
    {
        try
        {
            var phoneNoExist = await _appDbContext.TblEmployees
                .AnyAsync(x =>
                    x.EmployeeCode != employeeCode
                    && x.PhoneNo == phoneNo
                    && !x.DeleteFlag);

            return phoneNoExist
                ? Result<bool>.Success(true, "Phone No already exists for another employee.")
                : Result<bool>.NotFoundError("No duplicate Phone No found.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking duplicate phone no");
            return Result<bool>.SystemError("An error occurred while checking phone no duplication.");
        }
    }

    public async Task<Result<EmployeEditProfileResponseModel>> EditProfile(EmployeeEditProfileRequestModel requestModel)
    {
        try
        {
            #region Validation
           
            if (requestModel.EmployeeCode.IsNullOrEmpty())
            {
                return Result<EmployeEditProfileResponseModel>.ValidationError("Employee code required.");
            }

            var employee = await _appDbContext.TblEmployees
                //.Include(e => e.RoleCode)
                .FirstOrDefaultAsync(e => e.EmployeeCode == requestModel.EmployeeCode && !e.DeleteFlag);

            if (employee is null)
            {
                return Result<EmployeEditProfileResponseModel>.NotFoundError("Employee not found.");
            }

            if (!requestModel.Email.IsNullOrEmpty() &&
                requestModel.Email != employee.Email &&
                await _appDbContext.TblEmployees.AnyAsync(e => e.Email == requestModel.Email && e.EmployeeCode != requestModel.EmployeeCode))
            {
                return Result<EmployeEditProfileResponseModel>.ValidationError("Email already exists.");
            }

            if (requestModel.ProfileImage is not null && requestModel.ProfileImage.Length > 0)
            {
                var upload = await EnumDirectory.ProfileImage.UploadFilesAsync([requestModel.ProfileImage]);
                if (upload is not null && upload.Count != 0)
                {
                    employee.ProfileImage = upload.First().FilePath;
                }
            }

            #endregion

            if (!string.IsNullOrWhiteSpace(requestModel.Name)) employee.Name = requestModel.Name;
            if (!string.IsNullOrWhiteSpace(requestModel.Email)) employee.Email = requestModel.Email;
            if (!string.IsNullOrWhiteSpace(requestModel.PhoneNo)) employee.PhoneNo = requestModel.PhoneNo;
            if (!string.IsNullOrWhiteSpace(requestModel.Gender)) employee.Gender = requestModel.Gender;

            employee.ModifiedAt = DateTime.UtcNow;
            employee.ModifiedBy = UserCode;
            _appDbContext.Update(employee);
            await _appDbContext.SaveChangesAsync();

            return Result<EmployeEditProfileResponseModel>.Success("Profile Updated Successfully");
        }
        catch (Exception ex)
        {
            _logger.LogExceptionError(ex);
            return Result<EmployeEditProfileResponseModel>.Error($"An error occurred while editing profile: {ex.Message}");
        }
    }

    public async Task<TblEmployee?> GetEmployeeByEmail(string employeeEmail)
    {
            var employee = await _appDbContext.TblEmployees
                .FirstOrDefaultAsync(e => e.Email.ToLower() == employeeEmail.ToLower() && e.DeleteFlag == false);
            return employee;
    }

    public async Task<bool> UpdateEmployee (TblEmployee employee)
    {
         _appDbContext.TblEmployees.Update(employee);
        var result = await _appDbContext.SaveChangesAsync();
        return result > 0;
    }


}