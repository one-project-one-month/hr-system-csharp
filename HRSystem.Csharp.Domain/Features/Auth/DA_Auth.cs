using HRSystem.Csharp.Domain.Features.Role;
using HRSystem.Csharp.Domain.Models.Auth;
using HRSystem.Csharp.Domain.Models.Employee;
using Microsoft.AspNetCore.Http;
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
            {
                return Result<AuthResponseModel>.InvalidDataError("Invalid Username or password");
            }

            if (!_jwtService.VerifyPassword(requestModel.Password, user.Password))
            {
                return Result<AuthResponseModel>.InvalidDataError("Invalid Username or password");
            }

            var token = _jwtService.GenerateJwtToken(user.Username, user.Email, user.EmployeeCode);

            var jwtId = _jwtService.getJwtIdFromToken(token);

            var role = await _role.GetByRoleCode(user.RoleCode);

            if (!role.IsSuccess)
                return Result<AuthResponseModel>.BadRequestError("Assign Role first!");

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
                AccessToken = token,
                RefreshToken = refreshToken.Token,
                User = new EmployeeResponseModel {
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
}