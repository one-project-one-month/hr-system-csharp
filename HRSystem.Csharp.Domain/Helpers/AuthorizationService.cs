using Microsoft.AspNetCore.Http;

namespace HRSystem.Csharp.Domain.Helpers;

public class AuthorizationService
{
    private readonly IHttpContextAccessor _contextAccessor;

    public AuthorizationService(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public string? UserCode => _contextAccessor.HttpContext?.User.FindFirst("UserCode")?.Value;
}