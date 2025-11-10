using HRSystem.Csharp.Domain.Features.Role;
using HRSystem.Csharp.Domain.Models.RoleMenuPermission;

namespace HRSystem.Csharp.Domain.Features.RoleMenuPermission;

public class BL_RoleMenuPermission
{
    private readonly DA_RoleMenuPermission _daRoleMenuPermission;
    private readonly DA_Role _daRole;
    private readonly DA_MenuGroup _daMenuGroup;
    private readonly DA_Menu _daMenuItem;

    public BL_RoleMenuPermission(DA_RoleMenuPermission daRoleMenuPermission, DA_Role daRole, DA_MenuGroup daMenuGroup,
        DA_Menu daMenuItem)
    {
        _daRoleMenuPermission = daRoleMenuPermission;
        _daRole = daRole;
        _daMenuGroup = daMenuGroup;
        _daMenuItem = daMenuItem;
    }

    public async Task<Result<MenuTreeResponseModel>> GetMenuTreeWithPermissionsAsync(MenuTreeRequestModel reqModel)
    {
        var result = await _daRoleMenuPermission.GetMenuTreeWithPermissionsAsync(reqModel);
        return result.IsError
            ? Result<MenuTreeResponseModel>.Error(result.Message)
            : Result<MenuTreeResponseModel>.Success(result.Data);
    }

    public async Task<Result<CreateRoleMenuPermissionResponseModel>> CreateRoleMenuPermission
        (CreateRoleMenuPermissionRequestModel reqModel)
    {
        var roleResult = await _daRole.GetByRoleCode(reqModel.RoleCode);
        if (!roleResult.IsSuccess)
        {
            return Result<CreateRoleMenuPermissionResponseModel>.NotFoundError("Role doesn't exist!");
        }

        foreach (var p in reqModel.MenuPermissions)
        {
            if (string.IsNullOrWhiteSpace(p.MenuGroupCode))
            {
                return Result<CreateRoleMenuPermissionResponseModel>.Error("MenuGroupCode is required.");
            }

            if (!string.IsNullOrWhiteSpace(p.MenuItemCode))
            {
                var menu = await _daMenuItem.GetMenuByCode(p.MenuGroupCode);

                if (menu == null)
                    return Result<CreateRoleMenuPermissionResponseModel>.Error(
                        $"Invalid MenuCode '{p.MenuItemCode}' for group '{p.MenuGroupCode}'.");
            }

            var menuGroup = await _daMenuGroup.GetMenuGroupByCode(p.MenuGroupCode);

            if (menuGroup == null)
                return Result<CreateRoleMenuPermissionResponseModel>.Error(
                    $"MenuGroupCode '{p.MenuGroupCode}' does not exist.");
        }

        var result = await _daRoleMenuPermission.SaveRoleMenuPermissionsAsync(reqModel);
        return result.IsError
            ? Result<CreateRoleMenuPermissionResponseModel>.Error(result.Message)
            : Result<CreateRoleMenuPermissionResponseModel>.Success(result.Data);
    }
}