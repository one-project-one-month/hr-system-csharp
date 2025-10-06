using HRSystem.Csharp.Domain.Models;
using HRSystem.Csharp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Csharp.Domain.Features
{
    public class DA_Menu
    {

        private readonly AppDbContext _appDbContext;
        public DA_Menu(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Result<TblMenu>> CreateMenuAsync(Menu requestMenu)
        {
            try
            {
                var foundMenu = await _appDbContext.TblMenus.FirstOrDefaultAsync(
                    x => x.MenuCode == requestMenu.MenuCode && x.MenuGroupCode == requestMenu.MenuGroupCode);
                if (foundMenu != null)
                {
                    return Result<TblMenu>.DuplicateRecordError("Menu with that MenuCode or MenuGroupCode is existed", foundMenu);
                }

                // create new
                var menu = new TblMenu
                {
                    MenuId = Guid.NewGuid().ToString(),
                    MenuCode = requestMenu.MenuCode,
                    MenuName = requestMenu.MenuName,
                    MenuGroupCode = requestMenu.MenuGroupCode,
                    Url = requestMenu.Url,
                    Icon = requestMenu.Icon,
                    SortOrder = requestMenu.SortOrder,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = requestMenu.CreatedBy,
                    DeleteFlag = false,
                };

                await _appDbContext.TblMenus.AddAsync(menu);
                var result = await _appDbContext.SaveChangesAsync();
                if (result <= 0)
                {
                    return Result<TblMenu>.Error("Failed to create Menu.");
                }
                return Result<TblMenu>.Success(menu);
            }catch(Exception ex)
            {
                return Result<TblMenu>.Error($"An error is occured while creating Menu: {ex.Message}");
            }

        }


        public async Task<Result<TblMenu>> DeleteMenuAsync(string menuCode)
        {
            try
            {
                var foundMenu = _appDbContext.TblMenus.FirstOrDefault(x => x.MenuCode == menuCode);
                if (foundMenu is null)
                {
                    return Result<TblMenu>.NotFoundError();
                }

                foundMenu.ModifiedAt = DateTime.UtcNow;
                foundMenu.DeleteFlag = true;

                var result = await _appDbContext.SaveChangesAsync();
                if (result <= 0)
                {
                    return Result<TblMenu>.Error("Failed to delete Menu");
                }
                return Result<TblMenu>.Success(foundMenu);

            }catch(Exception ex)
            {
                return Result<TblMenu>.Error($"An error is occured while deleting the menu: {ex.Message}");
            }
        }
    }
}
