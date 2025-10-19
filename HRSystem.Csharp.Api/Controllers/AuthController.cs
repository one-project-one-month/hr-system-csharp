using HRSystem.Csharp.Database.AppDbContextModels;
using HRSystem.Csharp.Domain.Features.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace HRSystem.Csharp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;
        private readonly AppDbContext _appDbContext;
        private readonly BL_Auth _bl_Auth;

        public AuthController(JwtService jwtService, AppDbContext appDbContext, BL_Auth bL_Auth)
        {
            _jwtService = jwtService;
            _appDbContext = appDbContext;
            _bl_Auth = bL_Auth;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequestModel requestModel)
        {
            #region comment_code
            //var user = _appDbContext.TblEmployees.FirstOrDefault(x=>x.Username == requestModel.UserName);
            //if(user is null)
            //{
            //    return NotFound("Incorrect Username");
            //}
            //if(!_jwtService.VerifyPassword(requestModel.Password, user.Password))
            //{
            //    return BadRequest("Incorrect Password");
            //}

            //var token = _jwtService.GenerateJwtTokenAsync(user.Username, user.Email, user.EmployeeCode);

            //var jwtId = _jwtService.getJwtIdFromToken(token);

            //var refreshToken = new TblRefreshToken
            //{
            //    JwtId = jwtId,
            //    Token = Ulid.NewUlid().ToString(),
            //    EmployeeCode = user.EmployeeCode,
            //    Invalidated = false,
            //    IsRevoked = false,
            //    CreatedAt = DateTime.Now,
            //    CreatedBy = user.EmployeeCode,
            //    ExpiryDate = DateTime.Now.AddDays(7),
            //    DeleteFlag = false
            //};

            //_appDbContext.TblRefreshTokens.Add(refreshToken);
            //await _appDbContext.SaveChangesAsync();

            //var response = new AuthResponseModel
            //{
            //    AccessToken = token,
            //    RefreshToken = refreshToken.Token,
            //    ExpiresAt = new JwtSecurityTokenHandler().ReadJwtToken(token).ValidTo,
            //};
            #endregion

            var response = await _bl_Auth.LoginAsync(requestModel);
            return Ok(response);
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequestModel requestModel)
        {
            #region comment_code
            //var principal = _jwtService.GetPrincipalFromToken(requestModel.AccessToken);
            //if (principal is null) return BadRequest("Invalid access token.");

            //var jwtId = principal.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)?.Value;
            //if (jwtId is null) return BadRequest("Invalid Jwtid.");

            //var storedToken = _appDbContext.TblRefreshTokens.FirstOrDefault(x => x.Token == requestModel.RefreshToken);

            //if (storedToken is null) return NotFound("Refresh Token Not found.");

            //if (storedToken.Invalidated || storedToken.IsRevoked) return BadRequest("Refresh has been revoked.");

            //if (storedToken.ExpiryDate < DateTime.UtcNow) return BadRequest("Refresh Token has been expired.");

            //if (storedToken.JwtId != jwtId) return BadRequest("Access Token and Refresh Token are not matched.");

            //storedToken.Invalidated = true;
            //storedToken.IsRevoked = true;

            //var user = await _appDbContext.TblEmployees.FirstOrDefaultAsync(x => x.EmployeeCode == storedToken.EmployeeCode);

            //var newToken = _jwtService.GenerateJwtTokenAsync(user.Username, user.Email, user.EmployeeCode);

            //jwtId = _jwtService.getJwtIdFromToken(newToken);

            //var refreshToken = new TblRefreshToken
            //{
            //    JwtId = jwtId,
            //    Token = Ulid.NewUlid().ToString(),
            //    EmployeeCode = user.EmployeeCode,
            //    Invalidated = false,
            //    IsRevoked = false,
            //    CreatedAt = DateTime.Now,
            //    CreatedBy = user.EmployeeCode,
            //    ExpiryDate = DateTime.Now.AddDays(7),
            //    DeleteFlag = false
            //};

            //_appDbContext.TblRefreshTokens.Add(refreshToken);
            //await _appDbContext.SaveChangesAsync();

            //var response = new AuthResponseModel
            //{
            //    AccessToken = newToken,
            //    RefreshToken = refreshToken.Token,
            //    ExpiresAt = new JwtSecurityTokenHandler().ReadJwtToken(newToken).ValidTo,
            //};
            #endregion

            var response = _bl_Auth.RefreshTokenAsync(requestModel);
            return Ok(response);
        }

        [HttpPost("Logout")]
        [Authorize]
        public async Task<IActionResult> Logout (LogoutRequestModel requestModel)
        {
            var response = await _bl_Auth.LogoutAsync(requestModel);
            return Ok(response);
        }

        [HttpGet("TestAuthorize")]
        [Authorize]
        public IActionResult TestAuthorize()
        {
            return Ok("Authorization Successful!");
        }
                
        [HttpGet("HashPassword")]
        public IActionResult HashPassword (string password)
        {
            var hashPassword = _jwtService.HashPassword(password);
            return Ok(hashPassword);
        }
    }
}
