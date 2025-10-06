using HRSystem.Csharp.Domain.Models;
using HRSystem.Csharp.Shared;
using Microsoft.EntityFrameworkCore;
using NUlid;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Text.RegularExpressions;
using System.Threading.Tasks;

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

        public async Task<Result<Menu>> GetMenu(string menuId)
        {
            Menu? menu = await _daMenu.GetMenuById(menuId);
            if(menu == null)
            {
                return Result<Menu>.Error("Menu not found.");
            }
            return Result<Menu>.Success(menu);
        }

        public async Task<Result<bool>> UpdateMenu(string menuId, MenuRequestModel menu)
        {
            var result = await _daMenu.GetMenuById(menuId);
            if (result == null)
            {
                return Result<bool>.NotFoundError();
            }

            var duplicateMenu = await _daMenu.MenuExists(menuId, menu);
            if (duplicateMenu)
            {
                return Result<bool>.DuplicateRecordError("Menu with that MenuCode or MenuName is existed");
            }

            TblMenu updatingMenu = new TblMenu
            {
                MenuId = menuId,
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

        public async Task<Result<TblMenu>> CreateMenuAsync(Menu requestMenu)
        {
            if(requestMenu.MenuCode is null)
            {
                return Result<TblMenu>.BadRequestError("MenuCode is required.");
            }

            if(requestMenu.MenuGroupCode is null)
            {
                return Result<TblMenu>.BadRequestError("MenuGroupCode is required.");
            }

            if(requestMenu is null)
            {
                return Result<TblMenu>.BadRequestError("Request Menu cannot be null.");
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
