using HRSystem.Csharp.Domain.Features.Role;
using HRSystem.Csharp.Domain.Features.RoleMenuPermission;
using HRSystem.Csharp.Domain.Features.Sequence;
using HRSystem.Csharp.Domain.Models.Auth;
using HRSystem.Csharp.Domain.Models.Employee;
using HRSystem.Csharp.Domain.Models.RoleMenuPermission;
using HRSystem.Csharp.Shared;
using HRSystem.Csharp.Shared.Enums;
using HRSystem.Csharp.Database.AppDbContextModels;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.IdentityModel.Tokens.Jwt;

namespace HRSystem.Csharp.Domain.Features.Auth;

public class DA_Auth : AuthorizationService
{
    private readonly AppDbContext _appDbContext;
    private readonly JwtService _jwtService;
    private readonly DA_Role _role;
    private readonly DA_RoleMenuPermission _roleMenuPermission;
    private readonly DA_Sequence _daSequence;

    public DA_Auth(IHttpContextAccessor contextAccessor,
            JwtService jwtService, 
            AppDbContext appDbContext,
            DA_Role role,
            DA_RoleMenuPermission roleMenuPermission,
            DA_Sequence daSequence) : base(contextAccessor)
    {
        _jwtService = jwtService;
        _appDbContext = appDbContext;
        _role = role;
        _roleMenuPermission = roleMenuPermission;
        _daSequence = daSequence;
    }

    public async Task<Result<AuthResponseModel>> LoginAsync(LoginRequestModel requestModel)
    {
        try
        {
            if (string.IsNullOrEmpty(requestModel.UserName))
            {
                return Result<AuthResponseModel>.ValidationError("Username cannot be blank or empty.");
            }

            if (string.IsNullOrEmpty(requestModel.Password))
            {
                return Result<AuthResponseModel>.ValidationError("Password cannot be blank or empty.");
            }

            var hashPass = _jwtService.HashPassword(requestModel.Password);
            var user = _appDbContext.TblEmployees.FirstOrDefault(x => x.Username == requestModel.UserName);

            if (user is null)
                return Result<AuthResponseModel>.InvalidDataError("Invalid Username or password");


            if (!_jwtService.VerifyPassword(requestModel.Password, user.Password))
                return Result<AuthResponseModel>.InvalidDataError("Invalid Username or password");


            if (string.IsNullOrEmpty(user.RoleCode))
                return Result<AuthResponseModel>.InvalidDataError("The user doesn't have an assigned role!");

            var role = await _role.GetByRoleCode(user.RoleCode);
            if (!role.IsSuccess)
                return Result<AuthResponseModel>.InvalidDataError("The user doesn't have an assigned role!");

            if (user.IsFirstTimeLogin)
            {
                return Result<AuthResponseModel>.Success(
                    new AuthResponseModel
                    {
                        User = new EmployeeResponseModel
                        {
                            EmployeeCode = user.EmployeeCode,
                            Username = user.Username,
                            Name = user.Name,
                            Email = user.Email,
                            PhoneNo = user.PhoneNo,
                            RoleName = role.Data.RoleName,
                        }
                    }, 
                    "User is First Time.");
            }

            var token = _jwtService.GenerateJwtToken(user.Username, user.Email, user.EmployeeCode);

            var jwtId = _jwtService.getJwtIdFromToken(token);

            var model = new MenuTreeRequestModel
            {
                RoleCode = role.Data.RoleCode
            };
            
            var roleMenuPermission = await  _roleMenuPermission.GetMenuTreeWithPermissionsAsync(model);

            if(roleMenuPermission is null)
                return Result<AuthResponseModel>.InvalidDataError("Employee needs permissions to access");

            var refreshToken = new TblRefreshToken
            {
                JwtId = jwtId,
                Token = Ulid.NewUlid().ToString(),
                EmployeeCode = user.EmployeeCode,
                IsRevoked = false,
                CreatedAt = DateTime.Now,
                CreatedBy = user.EmployeeCode,
                ExpiryDate = DateTime.Now.AddDays(7),
                DeleteFlag = false,
                
            };

            _appDbContext.TblRefreshTokens.Add(refreshToken);
            await _appDbContext.SaveChangesAsync();
            Console.WriteLine("refresh token is ________________" + refreshToken.ToString());
            Console.WriteLine("jwtId is -------------" + jwtId);
            var response = new AuthResponseModel
            {
                AccessToken = token,
                RefreshToken = refreshToken.Token,
                User = new EmployeeResponseModel
                {
                    ProfileImage = user.ProfileImage,
                    EmployeeCode = user.EmployeeCode,
                    Username = user.Username,
                    RoleName = role.Data.RoleName,
                    Name = user.Name,
                    Email = user.Email,
                    PhoneNo = user.PhoneNo,
                    MenuTree = roleMenuPermission.Data,
                },
                ExpiresAt = new JwtSecurityTokenHandler().ReadJwtToken(token).ValidTo,
            };

            return Result<AuthResponseModel>.Success(response);
        }
        catch (Exception ex)
        {
            return Result<AuthResponseModel>.SystemError(ex.Message);
        }
    }

    public async Task<Result<AuthResponseModel>> RefreshTokenAsync(RefreshTokenRequestModel requestModel)
    {
        try
        {
            var storedToken = _appDbContext.TblRefreshTokens.FirstOrDefault(x => x.Token == requestModel.RefreshToken);

            if (storedToken is null)
            {
                return Result<AuthResponseModel>.ValidationError("Refresh Token Not found.");
            }

            if (storedToken.IsRevoked)
            {
                return Result<AuthResponseModel>.ValidationError("Refresh has been revoked.");
            }

            if (storedToken.ExpiryDate < DateTime.UtcNow)
            {
                return Result<AuthResponseModel>.ValidationError("Refresh Token has been expired.");
            }

            storedToken.IsRevoked = true;
            storedToken.RevokedAt = DateTime.UtcNow;

            var user = await _appDbContext.TblEmployees.FirstOrDefaultAsync(x =>
                x.EmployeeCode == storedToken.EmployeeCode && !x.DeleteFlag);
            if (user is null)
                Result<AuthResponseModel>.NotFoundError("User not found");

            var role = await _role.GetByRoleCode(user.RoleCode);
            if (role is null)
                Result<AuthResponseModel>.NotFoundError("Role with the user not found");

            var newToken = _jwtService.GenerateJwtToken(user.Username, user.Email, user.EmployeeCode);

            var jwtId = _jwtService.getJwtIdFromToken(newToken);

            var refreshToken = new TblRefreshToken
            {
                JwtId = jwtId,
                Token = Ulid.NewUlid().ToString(),
                EmployeeCode = user.EmployeeCode,
                IsRevoked = false,
                CreatedAt = DateTime.Now,
                CreatedBy = user.EmployeeCode,
                ExpiryDate = DateTime.Now.AddDays(7),
                DeleteFlag = false
            };

            _appDbContext.TblRefreshTokens.Add(refreshToken);
            await _appDbContext.SaveChangesAsync();

            var response = new AuthResponseModel
            {
                AccessToken = newToken,
                RefreshToken = refreshToken.Token,
                User = new EmployeeResponseModel
                {
                    ProfileImage = user.ProfileImage,
                    EmployeeCode = user.EmployeeCode,
                    Username = user.Username,
                    RoleName = role.Data.RoleName,
                    Name = user.Name,
                    Email = user.Email,
                    PhoneNo = user.PhoneNo
                },
                ExpiresAt = new JwtSecurityTokenHandler().ReadJwtToken(newToken).ValidTo,
            };

            return Result<AuthResponseModel>.Success(response);
        }
        catch (Exception ex)
        {
            return Result<AuthResponseModel>.SystemError(ex.Message);
        }
    }

    public async Task<Result<string>> LogoutAsync(LogoutRequestModel requestModel)
    {
        try
        {
            var tokens = await _appDbContext.TblRefreshTokens
                .Where(x => x.EmployeeCode == UserCode && x.IsRevoked != true)
                .ToListAsync();

            foreach (var token in tokens)
            {
                token.IsRevoked = true;
                token.ModifiedBy = UserCode;
                token.ModifiedAt = DateTime.Now;
            }

            await _appDbContext.SaveChangesAsync();

            return Result<string>.Success("Logout Successful.");
        }
        catch (Exception ex)
        {
            return Result<string>.SystemError(ex.Message);
        }
    }

    public async Task<Result<bool>> ChangePassword(ChangePasswordRequestModel requestModel)
    {
        if (requestModel is null)
        {
            return Result<bool>.ValidationError("Request cannot be null.");
        }

        if (string.IsNullOrEmpty(requestModel.EmployeeCode))
        {
            return Result<bool>.ValidationError("Employee code is required.");
        }

        if (string.IsNullOrEmpty(requestModel.OldPassword))
        {
            return Result<bool>.ValidationError("Old password is required.");
        }

        if (string.IsNullOrEmpty(requestModel.NewPassword))
        {
            return Result<bool>.ValidationError("New password is required.");
        }

        if (string.IsNullOrEmpty(requestModel.ConfirmPassword))
        {
            return Result<bool>.ValidationError("Confirm password is required.");
        }

        if (requestModel.NewPassword != requestModel.ConfirmPassword)
        {
            return Result<bool>.ValidationError("New password and confirm password do not match.");
        }

        if (requestModel.NewPassword.Length < 6)
        {
            return Result<bool>.ValidationError("New password must be at least 6 characters long.");
        }

        var user = await _appDbContext.TblEmployees
            .FirstOrDefaultAsync(x => x.EmployeeCode == requestModel.EmployeeCode);

        if (user is null)
        {
            return Result<bool>.NotFoundError("User not found.");
        }

        if (!_jwtService.VerifyPassword(requestModel.OldPassword, user.Password))
        {
            return Result<bool>.ValidationError("Old password is incorrect.");
        }

        user.Password = _jwtService.HashPassword(requestModel.NewPassword);
        user.IsFirstTimeLogin = false;

        _appDbContext.TblEmployees.Update(user);
        var result = await _appDbContext.SaveChangesAsync();

        return result > 0
            ? Result<bool>.Success(true, "Password updated. Please login again.")
            : Result<bool>.SystemError("Failed to update user.");
    }

    public async Task<Result<AuthResponseModel>> AutoLoginAsync()
    {
        try
        {
            const string adminUsername = "admin";
            const string adminPassword = "Admin123!";
            const string adminRoleCode = "RL001";
            const string adminRoleName = "Administrator";

            // Check if admin user exists
            var adminUser = await _appDbContext.TblEmployees
                .FirstOrDefaultAsync(x => x.Username == adminUsername && !x.DeleteFlag);

            // If admin user doesn't exist, create it
            if (adminUser == null)
            {
                // Check if admin role exists, if not create it
                var adminRole = await _role.GetByRoleCode(adminRoleCode);
                if (!adminRole.IsSuccess)
                {
                    // Create admin role
                    var newRole = new TblRole
                    {
                        RoleId = DevCode.GenerateNewUlid(),
                        RoleCode = adminRoleCode,
                        RoleName = adminRoleName,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = "SYSTEM",
                        DeleteFlag = false
                    };

                    _appDbContext.TblRoles.Add(newRole);
                    await _appDbContext.SaveChangesAsync();
                }

                // Generate employee code
                string employeeCode;
                try
                {
                    employeeCode = await _daSequence.GenerateCodeAsync(EnumSequenceCode.EMP.ToString());
                }
                catch
                {
                    // If sequence generation fails, use a default code
                    employeeCode = "EMP001";
                }

                // Create admin user
                var hashedPassword = _jwtService.HashPassword(adminPassword);
                adminUser = new TblEmployee
                {
                    EmployeeId = DevCode.GenerateNewUlid(),
                    EmployeeCode = employeeCode,
                    RoleCode = adminRoleCode,
                    Username = adminUsername,
                    Name = "System Administrator",
                    Email = "admin@hrsystem.com",
                    Password = hashedPassword,
                    PhoneNo = "+1234567890",
                    ProfileImage = "Profile",
                    StartDate = DateTime.UtcNow,
                    ResignDate = null,
                    Salary = 100000.00m,
                    IsFirstTimeLogin = false,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "SYSTEM",
                    DeleteFlag = false
                };

                _appDbContext.TblEmployees.Add(adminUser);
                await _appDbContext.SaveChangesAsync();
            }

            // Now perform login with admin credentials
            var loginRequest = new LoginRequestModel
            {
                UserName = adminUsername,
                Password = adminPassword
            };

            return await LoginAsync(loginRequest);
        }
        catch (Exception ex)
        {
            return Result<AuthResponseModel>.SystemError($"An error occurred during auto-login: {ex.Message}");
        }
    }
}