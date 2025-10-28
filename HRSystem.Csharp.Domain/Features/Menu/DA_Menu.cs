using HRSystem.Csharp.Domain.Models.Menu;
using Microsoft.AspNetCore.Http.HttpResults;

namespace HRSystem.Csharp.Domain.Features.Menu;

public class DA_Menu
{
    private readonly AppDbContext _dbContext;
    public DA_Menu(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<List<MenuModel>>> GetAllMenus()

    {
        try
        {
            var menus = await _dbContext.TblMenus
                .Join(_dbContext.TblMenuGroups,
                menu => menu.MenuGroupCode,
                menuGroup => menuGroup.MenuGroupCode,
                (menu, menuGroup) => new { menu, menuGroup })
                .Where(m => m.menu.DeleteFlag == false && m.menuGroup.DeleteFlag == false)
                .Select(m => new MenuModel
                {
                    MenuId = m.menu.MenuId,
                    MenuName = m.menu.MenuName,
                    MenuCode = m.menu.MenuCode,
                    MenuGroupCode = m.menu.MenuGroupCode,
                    Url = m.menu.Url,
                    Icon = m.menu.Icon,
                    CreatedAt = m.menu.CreatedAt,
                    ModifiedAt = m.menu.ModifiedAt,
                    SortOrder = m.menu.SortOrder,
                })
                .AsNoTracking()
                .ToListAsync();
           
            return Result<List<MenuModel>>.Success(menus);
        }
        catch (Exception ex)
        {
            return Result<List<MenuModel>>.Error($"An error occurred while retrieving menus: {ex.Message}");
        }
    }

    public async Task<MenuModel?> GetMenuByCode(string menuCode)
    {
        return await _dbContext.TblMenus
                .Join(_dbContext.TblMenuGroups,
                menu => menu.MenuGroupCode, 
                menuGroup => menuGroup.MenuGroupCode,
                (menu, menuGroup) => new { menu, menuGroup })
                .Where(m => m.menu.MenuCode.Equals(menuCode) && m.menu.DeleteFlag == false && m.menuGroup.DeleteFlag == false)
                .Select(m => new MenuModel
                {
                    MenuId = m.menu.MenuId,
                    MenuName = m.menu.MenuName,
                    MenuCode = m.menu.MenuCode,
                    MenuGroupCode = m.menu.MenuGroupCode,
                    Url = m.menu.Url,
                    Icon = m.menu.Icon,
                    CreatedAt = m.menu.CreatedAt,
                    ModifiedAt = m.menu.ModifiedAt,

                })
                .AsNoTracking()
                .SingleOrDefaultAsync();
    }

    public async Task<bool> MenuExists(string menuCode, MenuRequestModel menu)
    {
        return await (
            from m in _dbContext.TblMenus
            join g in _dbContext.TblMenuGroups on m.MenuGroupCode equals g.MenuGroupCode
            where
            m.MenuCode != menuCode && 
            (m.MenuCode == menu.MenuCode || m.MenuName == menu.MenuName) &&
            m.DeleteFlag == false && g.DeleteFlag == false &&
            m.MenuGroupCode == menu.MenuGroupCode
            select m).AnyAsync();

    }

    public async Task<bool> MenuCodeExists(MenuRequestModel menu)
    {
        return await (
            from m in _dbContext.TblMenus
            join g in _dbContext.TblMenuGroups on m.MenuGroupCode equals g.MenuGroupCode
            where
            (m.MenuCode == menu.MenuCode || m.MenuName == menu.MenuName) &&
            m.DeleteFlag == false && g.DeleteFlag == false &&
            m.MenuGroupCode == menu.MenuGroupCode
            select m).AnyAsync();
    }

    public async Task<bool> UpdateMenu(TblMenu menu)
    {

        _dbContext.TblMenus.Update(menu);
        int rows = await _dbContext.SaveChangesAsync();
        return rows > 0;
    }

    public async Task<Result<MenuModel>> CreateMenuAsync(MenuRequestModel requestMenu)
    {
        try
        {
            var foundMenu = await _dbContext.TblMenus.FirstOrDefaultAsync(
                x => x.MenuCode == requestMenu.MenuCode && x.MenuGroupCode == requestMenu.MenuGroupCode);
            if (foundMenu != null)
            {
                return Result<MenuModel>.DuplicateRecordError("Menu with that MenuCode or MenuGroupCode is existed");
            }

            // create new
            var menu = new TblMenu
            {
                MenuId = Ulid.NewUlid().ToString(),
                MenuCode = requestMenu.MenuCode,
                MenuName = requestMenu.MenuName,
                MenuGroupCode = requestMenu.MenuGroupCode,
                Url = requestMenu.Url,
                Icon = requestMenu.Icon,
                SortOrder = requestMenu.SortOrder,
                CreatedAt = DateTime.UtcNow,
                //CreatedBy =loggined User,
                DeleteFlag = false,
            };

            await _dbContext.AddAsync(menu);
            var result = await _dbContext.SaveChangesAsync();
            if (result <= 0)
            {
                return Result<MenuModel>.Error("Failed to create Menu.");
            }

            // map entity to DTO
            var menuModel = new MenuModel
            {
                MenuId = menu.MenuId,
                MenuCode = menu.MenuCode,
                MenuName = menu.MenuName,
                MenuGroupCode = menu.MenuGroupCode,
                Url = menu.Url,
                Icon = menu.Icon,
                SortOrder = menu.SortOrder,
                CreatedAt = menu.CreatedAt,
                //CreatedBy =loggined User,
                DeleteFlag = menu.DeleteFlag ?? false,
            };
            return Result<MenuModel>.Success(menuModel);
        }
        catch (Exception ex)
        {
            return Result<MenuModel>.Error($"An error is occured while creating Menu: {ex.Message}");
        }
    }

    public async Task<Result<bool>> DeleteMenuAsync(string menuCode)
    {
        try
        {
            var foundMenu = _dbContext.TblMenus.FirstOrDefault(x => x.MenuCode == menuCode);
            if (foundMenu is null)
            {
                return Result<bool>.NotFoundError();
            }

            foundMenu.ModifiedAt = DateTime.UtcNow;
            foundMenu.DeleteFlag = true;

            var result = await _dbContext.SaveChangesAsync();
            if (result <= 0)
            {
                return Result<bool>.Error("Failed to delete Menu");
            }
            return Result<bool>.Success(true);

        }
        catch (Exception ex)
        {
            return Result<bool>.Error($"An error is occured while deleting the menu: {ex.Message}");
        }
    }
}