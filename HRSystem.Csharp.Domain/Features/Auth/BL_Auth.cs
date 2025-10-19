using HRSystem.Csharp.Domain.Models.Auth;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Csharp.Domain.Features.Auth;

public class BL_Auth
{
    private readonly DA_Auth _da_Auth;

    public BL_Auth(DA_Auth da_Auth)
    {
        _da_Auth = da_Auth;
    }

    public async Task<Result<AuthResponseModel>> LoginAsync (LoginRequestModel loginRequest)
    {
        var response = await _da_Auth.LoginAsync(loginRequest);
        return response;
    }

    public async Task<Result<AuthResponseModel>> RefreshTokenAsync (RefreshTokenRequestModel requestModel)
    {
        var response = await _da_Auth.RefreshTokenAsync(requestModel);
        return response;
    }
    

    public async Task<Result<string>> LogoutAsync (LogoutRequestModel requestModel)
    {
        var response = await _da_Auth.LogoutAsync(requestModel);
        return response;
    }
}
