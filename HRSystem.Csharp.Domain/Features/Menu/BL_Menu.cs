using HRSystem.Csharp.Domain.Models.Common;
using HRSystem.Csharp.Domain.Models.Menu;

namespace HRSystem.Csharp.Domain.Features.Menu;

public class BL_Menu
{
    private readonly DA_Menu _daMenu;

    public BL_Menu(DA_Menu daMenu)
    {
        _daMenu = daMenu;
    }

    public async Task<Result<List<MenuModel>>> GetAllMenus(MenuPaginationModel model)
    {
        return await _daMenu.GetAllMenus(model);
    }

    public async Task<Result<MenuModel>> GetMenu(string menuCode)
    {
        MenuModel? menu = await _daMenu.GetMenuByCode(menuCode);
        if (menu == null)
            return Result<MenuModel>.NotFoundError("Menu not found.");

        return Result<MenuModel>.Success(menu);
    }

    public async Task<Result<bool>> UpdateMenu(string userId, TblMenu menu)
    {
        var existing = await _daMenu.GetMenuByCode(menu.MenuCode);
        if (existing == null)
            return Result<bool>.NotFoundError("Menu with this code doesn't exist!");

        bool menuGroupExist = await _daMenu.MenuGroupExists(menu.MenuGroupCode);

        if (!menuGroupExist)
            return Result<bool>.Error("Menu Group does not exist. Create Menu Group First!");

        var duplicateMenu = await _daMenu.MenuExists(menu);
        if (duplicateMenu)
            return Result<bool>.DuplicateRecordError("Menu with that MenuCode or MenuName already exists");

        TblMenu updating = new TblMenu
        {
            MenuId = existing.MenuId,
            MenuCode = existing.MenuCode,
            MenuName = menu.MenuName,
            MenuGroupCode = menu.MenuGroupCode,
            Url = menu.Url,
            Icon = menu.Icon,
            SortOrder = menu.SortOrder,
            ModifiedAt = DateTime.UtcNow,
            ModifiedBy = userId,
        };

        var updated = await _daMenu.UpdateMenu(updating);
        return updated
            ? Result<bool>.Success("Menu updated successfully!")
            : Result<bool>.Error("Update Menu Failed");
    }

    public async Task<Result<MenuModel>> CreateMenuAsync(string userId, MenuRequestModel requestMenu)
    {
        bool menuGroupExist = await _daMenu.MenuGroupExists(requestMenu.MenuGroupCode);
        if (!menuGroupExist)
            return Result<MenuModel>.Error("Menu Group does not exist. Create Menu Group First!");
       
        bool duplicateMenu = await _daMenu.MenuCodeExists(requestMenu);

        if (duplicateMenu)
            return Result<MenuModel>.DuplicateRecordError("Menu with that MenuCode or MenuName already exists");

        var response = await _daMenu.CreateMenuAsync(userId, requestMenu);
        return response;
    }

    public async Task<Result<bool>> DeleteMenuAsync(string userId, string menuCode)
    {
        if (menuCode is null)
            return Result<bool>.BadRequestError("MenuCode is required.");

        var response = await _daMenu.DeleteMenuAsync(userId, menuCode);
        return response;
    }
}