using HRSystem.Csharp.Domain.Models;
using HRSystem.Csharp.Shared;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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


        public async Task<Result<TblMenuGroup>> CreateMenuGroupAsync(MenuGroup requestMenuGroup)
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
