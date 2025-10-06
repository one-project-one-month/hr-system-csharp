using HRSystem.Csharp.Domain.Models;
using HRSystem.Csharp.Shared;
using Microsoft.EntityFrameworkCore;
using NUlid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task<Result<Menu>> CreateMenu(MenuRequestModel menu)
        {
            var menugroup = await _daMenu.GetMenuByCode(menu.MenuCode);

            if (menugroup == null)
            {
                return Result<Menu>.Error("Menu group not found.");
            }

            var menuExistsResult = await _daMenu.MenuExists(menu.MenuCode);
            if (menuExistsResult)
            {
                return Result<Menu>.Error("Menu with the same code already exists.");
            }
            var newMenu = new TblMenu
            {
                MenuId = Ulid.NewUlid().ToString(),
                MenuCode = menu.MenuCode,
                MenuName = menu.MenuName,
                MenuGroupCode = menu.MenuGroupCode,
                Url = menu.Url,
                Icon = menu.Icon,
                SortOrder = menu.SortOrder,
                CreatedAt = DateTime.Now,
                //CreatedBy = menu.CreatedBy,
                DeleteFlag = false
            };

            await _daMenu.CreateMenu(newMenu);
            return Result<Menu>.Success(new Menu
            {
                MenuId = newMenu.MenuId,
                MenuName = newMenu.MenuName,
                MenuCode = newMenu.MenuCode,
                MenuGroupCode = newMenu.MenuGroupCode,
                Url = newMenu.Url,
                Icon = newMenu.Icon,
                SortOrder = newMenu.SortOrder,
                CreatedAt = DateTime.Now,
                CreatedBy = newMenu.CreatedBy,
                ModifiedAt = DateTime.Now,
            });
        }

        public async Task<Result<bool>> UpdateMenu(string menuId, MenuRequestModel menu)
        {
            var result = await _daMenu.GetMenuById(menuId);
            if (result == null)
            {
                return Result<bool>.NotFoundError();
            }
            TblMenu creatingMenu = new TblMenu
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
            
            await _daMenu.UpdateMenu(creatingMenu);
            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> DeleteMenu(string menuId)
        {
            var result = await _daMenu.GetMenuById(menuId);
            if (result == null)
            {
                return Result<bool>.NotFoundError();
            }
            result.DeleteFlag = true;
            TblMenu deleting = new TblMenu
            {
                MenuId = menuId,
                MenuCode = result.MenuCode,
                MenuName = result.MenuName,
                MenuGroupCode = result.MenuGroupCode,
                Url = result.Url,
                Icon = result.Icon,
                SortOrder = result.SortOrder,
                ModifiedAt = DateTime.Now,
                DeleteFlag = true,
                //ModifiedBy = result.ModifiedBy,
            };
            await _daMenu.DeleteMenu(deleting);
            return Result<bool>.Success(true);

        }

    }
}
