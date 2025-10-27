using HRSystem.Csharp.Domain.Models.RoleMenuPermission;

namespace HRSystem.Csharp.Domain.Features.RoleMenuPermission;

public class BL_RoleMenuPermission
{
    private readonly DA_RoleMenuPermission _daRoleMenuPermission;

    public BL_RoleMenuPermission(DA_RoleMenuPermission daRoleMenuPermission)
    {
        _daRoleMenuPermission = daRoleMenuPermission;
    }

    public async Task<Result<MenuTreeResponseModel>> GetMenuTreeAsync()
    {
        var result = await _daRoleMenuPermission.GetMenuTreeAsync();
        if (result.IsError)
            return Result<MenuTreeResponseModel>.Error(result.Message);

        return Result<MenuTreeResponseModel>.Success(result.Data);
    }
}