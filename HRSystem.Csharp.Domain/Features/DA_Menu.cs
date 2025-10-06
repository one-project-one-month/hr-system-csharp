using HRSystem.Csharp.Domain.Models;
using HRSystem.Csharp.Shared;
using NUlid;


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
       
        public async Task<Result<TblMenu>> CreateMenuAsync(Menu requestMenu)
        {
            try
            {
                var foundMenu = await _dbContext.TblMenus.FirstOrDefaultAsync(
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

                await _dbContext.TblMenus.AddAsync(menu);
                var result = await _dbContext.SaveChangesAsync();
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
                var foundMenu = _dbContext.TblMenus.FirstOrDefault(x => x.MenuCode == menuCode);
                if (foundMenu is null)
                {
                    return Result<TblMenu>.NotFoundError();
                }

                foundMenu.ModifiedAt = DateTime.UtcNow;
                foundMenu.DeleteFlag = true;

                var result = await _dbContext.SaveChangesAsync();
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
