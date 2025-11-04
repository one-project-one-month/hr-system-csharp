using HRSystem.Csharp.Domain.Features.Auth;
using HRSystem.Csharp.Shared;
using Microsoft.AspNetCore.Authorization;

namespace HRSystem.Csharp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly JwtService _jwtService;
    private readonly BL_Auth _bl_Auth;

    public AuthController(JwtService jwtService, BL_Auth bL_Auth)
    {
        _jwtService = jwtService;
        _bl_Auth = bL_Auth;
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginRequestModel requestModel)
    {
        var response = await _bl_Auth.LoginAsync(requestModel);
        if (!response.IsSuccess)
            return BadRequest(response);
        return Ok(response);
    }

    [HttpPost("RefreshToken")]
    public async Task<IActionResult> RefreshToken(RefreshTokenRequestModel requestModel)
    {
        var response = await _bl_Auth.RefreshTokenAsync(requestModel);
        return Ok(response);
    }

    [HttpPost("Logout")]
    [Authorize]
    public async Task<IActionResult> Logout(LogoutRequestModel requestModel)
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
    public IActionResult HashPassword(string password)
    {   
        var hashPassword = _jwtService.HashPassword(password);
        return Ok(hashPassword);
    }
}