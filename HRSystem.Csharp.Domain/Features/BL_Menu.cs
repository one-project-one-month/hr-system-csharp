using HRSystem.Csharp.Domain.Models;
using HRSystem.Csharp.Shared;
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
            var menus = await _daMenu.GetAllMenus();
            var response = Result<List<Menu>>.Success(menus.Data);
            return response;
        }

        public async Task<Result<Menu>> GetMenu(string menuId)
        {
            var menu = await _daMenu.GetMenu(menuId);
            var response = Result<Menu>.Success(menu.Data);
            return response;
        }

        public async Task<Result<Menu>> CreateMenu(MenuCreateModel menu)
        {
            var createdMenu = await _daMenu.CreateMenu(menu);
            var response = Result<Menu>.Success(createdMenu.Data);
            return response;
        }

        public async Task<Result<bool>> UpdateMenu(string menuId, MenuCreateModel menu)
        {
            var updatedMenu = await _daMenu.UpdateMenu(menuId, menu);
            var response = Result<bool>.Success(updatedMenu.Data);
            return response;
        }

        public async Task<Result<bool>> DeleteMenu(string menuId)
        {
            var deletedMenu = await _daMenu.DeleteMenu(menuId);
            var response = Result<bool>.Success(deletedMenu.Data);
            return response;
        }

    }
}
