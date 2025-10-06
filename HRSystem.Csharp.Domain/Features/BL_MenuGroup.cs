using HRSystem.Csharp.Domain.Models;
using HRSystem.Csharp.Shared;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<Result<MenuGroup>> CreateMenuGroup(MenuGroupRequestModel menuGroup)
        {
            return (menuGroup) switch
            {
                (null) => Result<MenuGroup>.BadRequestError("MenuGroup Data is required!"),
                ({ MenuGroupCode: null or "" }) => Result<MenuGroup>.BadRequestError("MenuGroupCode is required!"),
                ({ MenuGroupName: null or "" }) => Result<MenuGroup>.BadRequestError("MenuGroupName is required!"),
                (_) => await _daMenuGroup.CreateMenuGroup(menuGroup)
            };
          
        }

        public async Task<Result<bool>> UpdateMenuGroup(string menuGroupId, MenuGroupRequestModel menuGroup)
        {
            return (menuGroupId, menuGroup) switch
            {
                (null or "", _) => Result<bool>.BadRequestError("MenuGroupId is required."),
                (_, null) => Result<bool>.BadRequestError("MenuGroup data is required."),
                (_, { MenuGroupCode: null or "" }) => Result<bool>.BadRequestError("MenuGroupCode is required."),
                (_, { MenuGroupName: null or "" }) => Result<bool>.BadRequestError("MenuGroupName is required."),
                _ => await _daMenuGroup.UpdateMenuGroup(menuGroupId, menuGroup)
            };

        }

        public async Task<Result<bool>> DeleteMenuGroup(string menuGroupId)
        {
            if(menuGroupId == null || menuGroupId == "")
            {
                return Result<bool>.BadRequestError("MenuGroupId is required.");
            }
            return await _daMenuGroup.DeleteMenuGroup(menuGroupId);
        }
    }
}
