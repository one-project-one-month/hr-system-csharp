using HRSystem.Csharp.Domain.Features.Role;
using HRSystem.Csharp.Domain.Models.Auth;
using HRSystem.Csharp.Domain.Models.Employee;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.IdentityModel.Tokens.Jwt;

namespace HRSystem.Csharp.Domain.Features.Auth;

public class DA_Auth : AuthorizationService
{
    private readonly AppDbContext _appDbContext;
    private readonly JwtService _jwtService;
    private readonly DA_Role _role;

    public DA_Auth(IHttpContextAccessor contextAccessor, JwtService jwtService, AppDbContext appDbContext, DA_Role role) : base(contextAccessor)
    {
        _jwtService = jwtService;
        _appDbContext = appDbContext;
        _role = role;
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

            _jwtService.HashPassword(requestModel.Password);
            var user = _appDbContext.TblEmployees.FirstOrDefault(x => x.Username == requestModel.UserName);

            if (user is null)
                return Result<AuthResponseModel>.InvalidDataError("Invalid Username or password");
            

            if (!_jwtService.VerifyPassword(requestModel.Password, user.Password))
                return Result<AuthResponseModel>.InvalidDataError("Invalid Username or password");
            

            if(string.IsNullOrEmpty(user.RoleCode))
                return Result<AuthResponseModel>.InvalidDataError("The user doen't have an assigned role!");

            var role = await _role.GetByRoleCode(user.RoleCode);
            if (!role.IsSuccess)
                return Result<AuthResponseModel>.InvalidDataError("The user doen't have an assigned role!");

            if (user.IsFirstTimeLogin == true)
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
                            PhoneNo = user.PhoneNo
                        } 
                    }, 
                    "User is First Time.");
            }

            var token = _jwtService.GenerateJwtToken(user.Username, user.Email, user.EmployeeCode);

            var jwtId = _jwtService.getJwtIdFromToken(token);

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
            Console.WriteLine("refresh token is ________________"+ refreshToken.ToString());
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
                    PhoneNo = user.PhoneNo
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
            var principal = _jwtService.GetPrincipalFromToken(requestModel.AccessToken);
            if (principal is null)
            {
                return Result<AuthResponseModel>.ValidationError("Invalid access token.");
            }

            var jwtId = principal.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)?.Value;
            if (jwtId is null)
            {
                return Result<AuthResponseModel>.ValidationError("Invalid Jwtid.");
            }

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

            if (storedToken.JwtId != jwtId)
            {
                return Result<AuthResponseModel>.ValidationError("Access Token and Refresh Token are not matched.");
            }

            storedToken.IsRevoked = true;
            storedToken.RevokedAt = DateTime.UtcNow;

            var user = await _appDbContext.TblEmployees.FirstOrDefaultAsync(x => x.EmployeeCode == storedToken.EmployeeCode && !x.DeleteFlag);
            if (user is null)
                Result<AuthResponseModel>.NotFoundError("User not found");

            var role = await _role.GetByRoleCode(user.RoleCode);
            if (role is null)
                Result<AuthResponseModel>.NotFoundError("Role with the user not found");

            var newToken = _jwtService.GenerateJwtToken(user.Username, user.Email, user.EmployeeCode);

            jwtId = _jwtService.getJwtIdFromToken(newToken);

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

}