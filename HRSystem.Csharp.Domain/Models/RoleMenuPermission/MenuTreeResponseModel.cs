namespace HRSystem.Csharp.Domain.Models.RoleMenuPermission;

public class MenuTreeResponseModel
{
    public List<MenuGroupResponseModel> MenuTree { get; set; }
}

public class MenuGroupResponseModel
{
    public string MenuGroupCode { get; set; }
    public string MenuGroupName { get; set; }
    public string MenuGroupIcon { get; set; }
    public string MenuGroupUrl { get; set; }
    public bool IsChecked { get; set; }
    public List<MenuItemResponseModel> ChildMenus { get; set; }
}

public class MenuItemResponseModel
{
    public string MenuItemCode { get; set; }
    public string MenuItemName { get; set; }
    public string MenuItemIcon { get; set; }
    public string MenuItemUrl { get; set; }
    public bool IsChecked { get; set; }
}

public class MenuTreeRequestModel
{
    public string? RoleCode { get; set; }
}