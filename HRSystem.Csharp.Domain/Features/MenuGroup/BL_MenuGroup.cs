using HRSystem.Csharp.Domain.Models.Common;
using HRSystem.Csharp.Domain.Models.MenuGroup;

namespace HRSystem.Csharp.Domain.Features.MenuGroup;

public class BL_MenuGroup
{
    private readonly DA_MenuGroup _daMenuGroup;
    public BL_MenuGroup(DA_MenuGroup daMenuGroup)
    {
        _daMenuGroup = daMenuGroup;
    }

    public async Task<Result<List<MenuGroupModel>>> GetAllMenuGroups(PaginationRequestModel model)
    {
        var response = await _daMenuGroup.GetAllMenuGroups(model);
        return Result<List<MenuGroupModel>>.Success(response);
    }

    public async Task<Result<MenuGroupModel>> GetMenuGroup(string menuGroupId)
    {
        if (string.IsNullOrEmpty(menuGroupId))
        {
            return Result<MenuGroupModel>.BadRequestError("MenuGroupId is required.");
        }
        var menuGroup = await _daMenuGroup.GetMenuGroupByCode(menuGroupId);
        if (menuGroup == null)
        {
            return Result<MenuGroupModel>.Error("Menu group not found.");
        }
        return Result<MenuGroupModel>.Success(menuGroup);
    }

    public async Task<Result<bool>> UpdateMenuGroup(string menuGroupCode, MenuGroupUpdateRequestModel menuGroup)
    {
        return await _daMenuGroup.UpdateMenuGroup(menuGroupCode, menuGroup);
    }

    public async Task<Result<MenuGroupModel>> CreateMenuGroupAsync(MenuGroupRequestModel requestMenuGroup)
    {
        var response = await _daMenuGroup.CreateMenuGroupAsync(requestMenuGroup);
        return response;
    }

    public async Task<Result<bool>> DeleteMenuGroupAsync(string menuGroupCode)
    {
        if (menuGroupCode is null)
        {
            return Result<bool>.BadRequestError("Menu Group Code is required.");
        }
        var response = await _daMenuGroup.DeleteMenuGroup(menuGroupCode);
        return response;
    }
}