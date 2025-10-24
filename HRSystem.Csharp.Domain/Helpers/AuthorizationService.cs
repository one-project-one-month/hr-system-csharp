using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

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
