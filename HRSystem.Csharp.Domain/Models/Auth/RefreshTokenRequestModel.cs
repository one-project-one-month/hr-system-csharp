using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Csharp.Domain.Models.Auth;

public class RefreshTokenRequestModel
{
    public string AccessToken { get; set; }
    public string RefreshToken {  get; set; }
}
