using HRSystem.Csharp.Domain.Models;
using HRSystem.Csharp.Shared;
using NUlid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Csharp.Domain.Features
{
    public class DA_Menu
    {
        private readonly AppDbContext _dbContext;
        public DA_Menu(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<List<Menu>>> GetAllMenus()
        {
            try
            {
                var menus = await _dbContext.TblMenus
                    .Where(m => m.DeleteFlag == false)
                    .Select(m => new Menu
                    {
                        MenuId = m.MenuId,
                        MenuName = m.MenuName,
                        MenuCode = m.MenuCode,
                        MenuGroupCode = m.MenuGroupCode,
                        Url = m.Url,
                        Icon = m.Icon,
                        CreatedAt = m.CreatedAt,
                        ModifiedAt = m.ModifiedAt,
                        SortOrder = m.SortOrder,
                    })
                    .ToListAsync();
                return Result<List<Menu>>.Success(menus);
            }
            catch (Exception ex)
            {
                return Result<List<Menu>>.Error($"An error occurred while retrieving menus: {ex.Message}");
            }
        }

        public async Task<Result<Menu>> GetMenu(string menuId)
        {
            try
            {

                Menu? menu = await _dbContext.TblMenus
                    .Where(m => m.MenuId.Equals(menuId) && m.DeleteFlag == false)
                    .Select(m => new Menu
                    {
                        MenuId = m.MenuId,
                        MenuName = m.MenuName,
                        MenuCode = m.MenuCode,
                        MenuGroupCode = m.MenuGroupCode,
                        Url = m.Url,
                        Icon = m.Icon,
                        CreatedAt = m.CreatedAt,
                        ModifiedAt = m.ModifiedAt,

                    }).SingleOrDefaultAsync();

                if (menu == null)
                {
                    return Result<Menu>.NotFoundError("Menu not found.");
                }
                return Result<Menu>.Success(menu);
            }
            catch (Exception ex)
            {
                return Result<Menu>.Error($"An error occurred while retrieving menus: {ex.Message}");
            }

        }

        public async Task<Result<Menu>> CreateMenu(MenuCreateModel menu)
        {
            var menugroup = await _dbContext.TblMenuGroups
                .Where(mg => mg.MenuGroupCode.Equals(menu.MenuGroupCode) && mg.DeleteFlag == false)
                .FirstOrDefaultAsync();

            if(menugroup == null)
            {
                return Result<Menu>.Error("Menu group not found.");
            }

            _dbContext.TblMenus.Add(new TblMenu
            {
                MenuId = Ulid.NewUlid().ToString(),
                MenuCode= menu.MenuCode,
                MenuName = menu.MenuName,
                MenuGroupCode = menu.MenuGroupCode,
                Url = menu.Url,
                Icon = menu.Icon,
                SortOrder = menu.SortOrder,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
                DeleteFlag = false
            });
            await _dbContext.SaveChangesAsync();
            return Result<Menu>.Success(new Menu
            {
                MenuName = menu.MenuName,
                MenuCode = menu.MenuCode,
                MenuGroupCode = menu.MenuGroupCode,
                Url = menu.Url,
                Icon = menu.Icon,
                SortOrder = menu.SortOrder,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
            });
        }

        public async Task<Result<bool>> MenuExists(string menuCode)
        {
            var result =  await _dbContext.TblMenus.AnyAsync(m => m.MenuCode.Equals(menuCode) && m.DeleteFlag == false);
            return Result<bool>.Success(result);
        }

        public async Task<Result<bool>> UpdateMenu(string menuId, MenuCreateModel menu)
        {
            var result = await _dbContext.TblMenus
                .Where(m => m.MenuId.Equals(menuId) && m.DeleteFlag == false)
                .SingleOrDefaultAsync();

            if (result == null)
            { 
                return Result<bool>.NotFoundError(); 
            }

            result.MenuName = menu.MenuName;
            result.MenuCode = menu.MenuCode;
            result.MenuGroupCode = menu.MenuGroupCode;
            result.Url = menu.Url;
            result.Icon = menu.Icon;
            result.SortOrder = menu.SortOrder;
            _dbContext.TblMenus.Update(result);
            await _dbContext.SaveChangesAsync();

            return Result<bool>.Success(true);

        }
        public async Task<Result<bool>> DeleteMenu(string menuId)
        {
            var result = await _dbContext.TblMenus
                .Where(m => m.MenuId.Equals(menuId) && m.DeleteFlag == false)
                .SingleOrDefaultAsync();
            if (result == null)
            {
                return Result<bool>.NotFoundError();
            }
            result.DeleteFlag = true;
            _dbContext.TblMenus.Update(result);
            await _dbContext.SaveChangesAsync();
            return Result<bool>.Success(true);
        }
    }
}
