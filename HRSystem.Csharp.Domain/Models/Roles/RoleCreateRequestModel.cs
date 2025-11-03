namespace HRSystem.Csharp.Domain.Models.Roles;

public class RoleCreateRequestModel
{
    public string RoleId { get; set; } = null!;
    public string RoleName { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = null!;
}