namespace HRSystem.Csharp.Domain.Models.Verification;

public class VerificationResponseModel
{
    public List<VerificationModel> VerificationCodes { get; set; } = [];
    public VerificationModel VerificationCode { get; set; } = new();
}

public class ResetTokenResponse
{
    public string ResetToken { get; set; } = null!;
}
