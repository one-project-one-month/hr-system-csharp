namespace HRSystem.Csharp.Domain.Models.Verification;

public class VerificationModel
{
    public string VerificationId { get; set; } = string.Empty;
    public string VerificationCode { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime? ExpiredTime { get; set; }
    public bool? Isused { get; set; }
    public DateTime? Createdat { get; set; }
    public string Createdby { get; set; } = string.Empty;
    public DateTime? Modifiedat { get; set; }
    public string? Modifiedby { get; set; }
    public bool? Deleteflag { get; set; }
    public static VerificationModel FromTblVerification(TblVerification verification)
    {
        return new VerificationModel
        {
            VerificationId = verification.VerificationId,
            VerificationCode = verification.VerificationCode,
            Email = verification.Email,
            ExpiredTime = verification.ExpiredTime,
            Isused = verification.IsUsed,
            Createdby = verification.CreatedBy,
            Createdat = verification.CreatedAt,
            Modifiedat = verification.ModifiedAt,
            Modifiedby = verification.ModifiedBy,
            Deleteflag = verification.DeleteFlag,
        };
    }
}