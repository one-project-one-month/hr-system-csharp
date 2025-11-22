using HRSystem.Csharp.Domain.Models.Employee;
using HRSystem.Csharp.Domain.Models.RoleMenuPermission;

namespace HRSystem.Csharp.Domain.Models.Auth;

public class AuthResponseModel
{
    public string AccessToken { get; set; } = null!;

    public string RefreshToken { get; set; } = null!;

    public EmployeeResponseModel User { get; set; } = null!;

    public DateTime ExpiresAt { get; set; } 
}