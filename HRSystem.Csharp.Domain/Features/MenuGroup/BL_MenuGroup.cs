using HRSystem.Csharp.Domain.Models.MenuGroup;

namespace HRSystem.Csharp.Domain.Features.MenuGroup;

public class BL_MenuGroup
{
    private readonly DA_MenuGroup _daMenuGroup;
    public BL_MenuGroup(DA_MenuGroup daMenuGroup)
    {
        _daMenuGroup = daMenuGroup;
    }

    public async Task<Result<List<MenuGroupModel>>> GetAllMenuGroups()
    {
        var response = await _daMenuGroup.GetAllMenuGroups();
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

    public async Task<Result<TblMenuGroup>> CreateMenuGroupAsync(MenuGroupRequestModel requestMenuGroup)
    {
        if (requestMenuGroup.MenuGroupCode is null || string.IsNullOrWhiteSpace(requestMenuGroup.MenuGroupCode))
        {
            return Result<TblMenuGroup>.BadRequestError("MenuGroupCode cannot be null");
        }

        if (requestMenuGroup is null)
        {
            return Result<TblMenuGroup>.BadRequestError("Request MenuGroup cannot be null");
        }


        var response = await _daMenuGroup.CreateMenuGroupAsync(requestMenuGroup);
        return response;
    }

    public async Task<Result<TblMenuGroup>> DeleteMenuGroupAsync(string menuGroupCode)
    {
        if (menuGroupCode is null)
        {
            return Result<TblMenuGroup>.BadRequestError("Menu Group Code is required.");
        }
        var response = await _daMenuGroup.DeleteMenuGroup(menuGroupCode);
        return response;
    }
}