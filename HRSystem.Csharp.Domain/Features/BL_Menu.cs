using HRSystem.Csharp.Domain.Models;
using HRSystem.Csharp.Shared;
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
