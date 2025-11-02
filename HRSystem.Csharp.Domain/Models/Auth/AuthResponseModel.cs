namespace HRSystem.Csharp.Domain.Models.Auth;

public class AuthResponseModel
{
    public string AccessToken { get; set; } = null!;

    public string RefreshToken { get; set; } = null!;

    public string RoleName { get; set; } = null!;

    public DateTime ExpiresAt { get; set; } 
}