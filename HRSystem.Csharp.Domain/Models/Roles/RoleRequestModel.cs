namespace HRSystem.Csharp.Domain.Models.Roles;

public class RoleRequestModel
{
    public string RoleName { get; set; } = null!;
}

public class RoleEditRequestModel
{
    public string RoleCode { get; set; } = string.Empty;
}

public class RoleUpdateRequestModel
{
    // public string RoleCode { get; set; }
    public string RoleName { get; set; } = string.Empty;
}

public class RoleDeleteRequestModel
{
    public string RoleCode { get; set; } = string.Empty;
}