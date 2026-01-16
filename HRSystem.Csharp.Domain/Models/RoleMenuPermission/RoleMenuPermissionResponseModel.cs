namespace HRSystem.Csharp.Domain.Models.RoleMenuPermission;

public class CreateRoleMenuPermissionResponseModel
{
    public List<CreateRoleMenuPermissionModel> RoleMenuPermissions { get; set; } = [];
}

public class CreateRoleMenuPermissionModel
{
    public string RoleAndMenuPermissionId { get; set; } = string.Empty;
    public string RoleAndMenuPermissionCode { get; set; } = string.Empty;
    public string RoleCode { get; set; } = string.Empty;
    public string MenuGroupCode { get; set; } = string.Empty;
    public string? PermissionCode { get; set; }
    public string? MenuCode { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public string CreatedUserId { get; set; } = string.Empty;
}

public class CreateRoleMenuPermissionRequestModel
{
    public string RoleCode { get; set; } = null!;
    public List<MenuPermissionRequestModel> MenuPermissions { get; set; } = [];
}

public class MenuPermissionRequestModel
{
    public string? MenuGroupCode { get; set; }
    public string? MenuItemCode { get; set; }
    public string? PermissionCode { get; set; }
    public bool IsChecked { get; set; }
}