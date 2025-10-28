using HRSystem.Csharp.Domain.Models.Menu;

namespace HRSystem.Csharp.Domain.Features.Menu;

public class BL_Menu
{
    private readonly DA_Menu _daMenu;
    public BL_Menu(DA_Menu daMenu)
    {
        _daMenu = daMenu;
    }

    public async Task<Result<List<MenuModel>>> GetAllMenus()
    {

        return await _daMenu.GetAllMenus();
    }

    public async Task<Result<MenuModel>> GetMenu(string menuCode)
    {
        MenuModel? menu = await _daMenu.GetMenuByCode(menuCode);
        if (menu == null)
        {
            return Result<MenuModel>.Error("Menu not found.");
        }
        return Result<MenuModel>.Success(menu);
    }

    public async Task<Result<bool>> UpdateMenu(string menuCode, MenuRequestModel menu)
    {
        var result = await _daMenu.GetMenuByCode(menuCode);
        if (result == null)
        {
            return Result<bool>.NotFoundError();
        }

        var duplicateMenu = await _daMenu.MenuExists(menuCode, menu);
        if (duplicateMenu)
        {
            return Result<bool>.DuplicateRecordError("Menu with that MenuCode or MenuName is existed");
        }

        TblMenu updatingMenu = new TblMenu
        {
            MenuId = result.MenuId,
            MenuCode = menu.MenuCode,
            MenuName = menu.MenuName,
            MenuGroupCode = menu.MenuGroupCode,
            Url = menu.Url,
            Icon = menu.Icon,
            SortOrder = menu.SortOrder,
            ModifiedAt = DateTime.UtcNow,
            //ModifiedBy = menu.ModifiedBy,
        };

        await _daMenu.UpdateMenu(updatingMenu);
        return Result<bool>.Success(true);
    }

    public async Task<Result<MenuModel>> CreateMenuAsync(MenuRequestModel requestMenu)
    {
        var duplicateMenu = await _daMenu.MenuCodeExists(requestMenu);

        if (duplicateMenu)
        {
            return Result<MenuModel>.DuplicateRecordError("Menu with that MenuCode or MenuName is existed");
        }

        var response = await _daMenu.CreateMenuAsync(requestMenu);
        return response;
    }

    public async Task<Result<bool>> DeleteMenuAsync(string menuCode)
    {
        if (menuCode is null)
        {
            return Result<bool>.BadRequestError("MenuCode is required.");
        }

        var response = await _daMenu.DeleteMenuAsync(menuCode);
        return response;
    }
}