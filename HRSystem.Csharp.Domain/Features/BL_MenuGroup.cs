using HRSystem.Csharp.Domain.Models;
using HRSystem.Csharp.Shared;


namespace HRSystem.Csharp.Domain.Features
{
    public class BL_MenuGroup
    {
        private readonly DA_MenuGroup _daMenuGroup;
        public BL_MenuGroup(DA_MenuGroup daMenuGroup)
        {
            _daMenuGroup = daMenuGroup;
        }
        public async Task<Result<List<MenuGroup>>> GetAllMenuGroups()
        {
            var response = await _daMenuGroup.GetAllMenuGroups();
            return Result<List<MenuGroup>>.Success(response);
        }

        public async Task<Result<MenuGroup>> GetMenuGroup(string menuGroupId)
        {
            if (string.IsNullOrEmpty(menuGroupId))
            {
                return Result<MenuGroup>.BadRequestError("MenuGroupId is required.");
            }
            var menuGroup = await _daMenuGroup.GetMenuGroupById(menuGroupId);
            if (menuGroup == null)
            {
                return Result<MenuGroup>.Error("Menu group not found.");
            }
            return Result<MenuGroup>.Success(menuGroup);
        }

        public async Task<Result<bool>> UpdateMenuGroup(string menuGroupId, MenuGroupUpdateRequestModel menuGroup)
        {
            return await _daMenuGroup.UpdateMenuGroup(menuGroupId, menuGroup);
        }
        
        public async Task<Result<TblMenuGroup>> CreateMenuGroupAsync(MenuGroupRequestModel requestMenuGroup)
        {
            if(requestMenuGroup.MenuGroupCode is null)
            {
                return Result<TblMenuGroup>.BadRequestError("MenuGroupCode cannot be null");
            }

            if(requestMenuGroup is null)
            {
                return Result<TblMenuGroup>.BadRequestError("Request MenuGroup cannot be null");
            }


            var response = await _daMenuGroup.CreateMenuGroupAsync(requestMenuGroup);
            return response;
        }


        public async Task<Result<TblMenuGroup>> DeleteMenuGroupAsync(string menuGroupCode)
        {
            if(menuGroupCode is null)
            {
                return Result<TblMenuGroup>.BadRequestError("Menu Group Code is required.");
            }
            var response = await _daMenuGroup.DeleteMenuGroup(menuGroupCode);
            return response;
        }
    }
}
