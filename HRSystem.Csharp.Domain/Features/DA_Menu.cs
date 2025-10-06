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

        public async Task<Menu?> GetMenuById(string menuId)
        {
            return await _dbContext.TblMenus
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
        }

        public async Task<Menu?> GetMenuByCode(string menuCode)
        {
            return await _dbContext.TblMenus
                    .Where(m => m.MenuCode.Equals(menuCode) && m.DeleteFlag == false)
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

        }
        public async Task<bool> CreateMenu(TblMenu menu)
        {
            
            _dbContext.TblMenus.Add(menu);
            int rows = await _dbContext.SaveChangesAsync();
            return rows > 0;
        }

        public async Task<bool> MenuExists(string menuCode)
        {
            return await _dbContext.TblMenus.AnyAsync(m => m.MenuCode.Equals(menuCode) && m.DeleteFlag == false);
           
        }

        public async Task<bool> UpdateMenu(TblMenu menu)
        {
            
            _dbContext.TblMenus.Update(menu);
            int rows = await _dbContext.SaveChangesAsync();
            return rows > 0;
        }
        public async Task<bool> DeleteMenu(TblMenu menu)
        {
            _dbContext.TblMenus.Update(menu);
            int rows = await _dbContext.SaveChangesAsync();
            return rows > 0;
        }
    }
}
