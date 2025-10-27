namespace HRSystem.Csharp.Domain.Models.RoleMenuPermission;

public class MenuTreeResponseModel
{
    public List<MenuGroupResponseModel> MenuTree { get; set; }
}

public class MenuGroupResponseModel
{
    public string GroupCode { get; set; }
    public string GroupName { get; set; }
    public string GroupIcon { get; set; }
    public string GroupUrl { get; set; }
    public List<MenuItemResponseModel> Items { get; set; }
}

public class MenuItemResponseModel
{
    public string MenuCode { get; set; }
    public string MenuName { get; set; }
    public string MenuIcon { get; set; }
    public string MenuUrl { get; set; }
}