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
