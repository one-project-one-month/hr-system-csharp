namespace HRSystem.Csharp.Domain.Models.Verification;

public class VerificationResponseModel
{
    public List<VerificationModel> VerificationCodes { get; set; } = new();
    public VerificationModel VerificationCode { get; set; }
}
