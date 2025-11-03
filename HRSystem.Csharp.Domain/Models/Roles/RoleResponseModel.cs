namespace HRSystem.Csharp.Domain.Models.Roles;

public class RoleResponseModel
{
    public string RoleId { get; set; } = null!;
    public string RoleCode { get; set; } = null!;
    public string RoleName { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = null!;
    public DateTime? ModifiedAt { get; set; }
    public string? ModifiedBy { get; set; }
    public bool DeleteFlag { get; set; }
}