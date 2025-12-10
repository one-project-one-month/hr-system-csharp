namespace HRSystem.Csharp.Domain.Models.Verification;

public class VerificationRequestModel
{
    public string? Email { get; set; }
    
}

public class VerifiyCodeRequestModel
{
    public string? Email { get; set; }
    public string? VerificationCode { get; set; }
}

public class ResetPasswordRequestModel
{
    public string? Email { get; set; } = string.Empty;
    public string ResetToken { get; set; } = null!;
    public string NewPassword { get; set; }= null!;
}