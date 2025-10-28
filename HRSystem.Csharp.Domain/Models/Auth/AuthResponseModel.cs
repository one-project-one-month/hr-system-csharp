namespace HRSystem.Csharp.Domain.Models.Auth;

public class AuthResponseModel
{
    public string AccessToken { get; set; }

    public string RefreshToken { get; set; }

    public DateTime ExpiresAt { get; set; } 
}