using System;
using System.Collections.Generic;

namespace HRSystem.Csharp.Database.AppDbContextModels;

public partial class TblVerification
{
    public Guid VerificationId { get; set; }

    public string? VerificationCode { get; set; }

    public string? Email { get; set; }

    public DateTime? ExpiredTime { get; set; }

    public bool? IsUsed { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public bool? DeleteFlag { get; set; }
}
