namespace HRSystem.Csharp.Domain.Models.RoleMenuPermission;

public class CreateRoleMenuPermissionResponseModel
{
    public List<CreateRoleMenuPermissionModel> RoleMenuPermissions { get; set; }
}

public class CreateRoleMenuPermissionModel
{
    public string RoleAndMenuPermissionId { get; set; }
    public string RoleAndMenuPermissionCode { get; set; }
    public string RoleCode { get; set; }
    public string MenuGroupCode { get; set; }
    public string PermissionCode { get; set; }
    public string? MenuCode { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public string CreatedUserId { get; set; }
}

public class CreateRoleMenuPermissionRequestModel
{
    public string RoleCode { get; set; }
    public List<MenuPermissionRequestModel> MenuPermissions { get; set; }
}

public class MenuPermissionRequestModel
{
    public string MenuGroupCode { get; set; }
    public string MenuItemCode { get; set; }
    public string PermissionCode { get; set; }
    public bool IsChecked { get; set; }
}