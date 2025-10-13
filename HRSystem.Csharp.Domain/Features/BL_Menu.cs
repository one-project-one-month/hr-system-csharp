using HRSystem.Csharp.Domain.Models;
using HRSystem.Csharp.Shared;


namespace HRSystem.Csharp.Domain.Features
{
    public class BL_Menu
    {
        private readonly DA_Menu _daMenu;
        public BL_Menu(DA_Menu daMenu)
        {
            _daMenu = daMenu;
        }

        public async Task<Result<List<Menu>>> GetAllMenus()
        {

            return await _daMenu.GetAllMenus();
        }

        public async Task<Result<Menu>> GetMenu(string menuCode)
        {
            Menu? menu = await _daMenu.GetMenuByCode(menuCode);
            if(menu == null)
            {
                return Result<Menu>.Error("Menu not found.");
            }
            return Result<Menu>.Success(menu);
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
                ModifiedAt = DateTime.Now,
                //ModifiedBy = menu.ModifiedBy,
            };
            
            await _daMenu.UpdateMenu(updatingMenu);
            return Result<bool>.Success(true);
        }

        public async Task<Result<TblMenu>> CreateMenuAsync(MenuRequestModel requestMenu)
        {
            if(requestMenu.MenuCode is null || string.IsNullOrWhiteSpace(requestMenu.MenuCode))
            {
                return Result<TblMenu>.BadRequestError("MenuCode cannot be empty.");
            }

            if(requestMenu.MenuGroupCode is null || string.IsNullOrWhiteSpace(requestMenu.MenuGroupCode))
            {
                return Result<TblMenu>.BadRequestError("MenuGroupCode cannot be empty.");
            }

            if(requestMenu is null)
            {
                return Result<TblMenu>.BadRequestError("Request Menu cannot be null.");
            }

            var duplicateMenu = await _daMenu.MenuCodeExists(requestMenu);

            if(duplicateMenu)
            {
                return Result<TblMenu>.DuplicateRecordError("Menu with that MenuCode or MenuName is existed");
            }

            var response = await _daMenu.CreateMenuAsync(requestMenu);
            return response;
        }

        

        public async Task<Result<TblMenu>> DeleteMenuAsync(string menuCode)
        {
            if(menuCode is null)
            {
                return Result<TblMenu>.BadRequestError("MenuCode is required.");
            }

            var response = await _daMenu.DeleteMenuAsync(menuCode);
            return response;
        }
    }
}
