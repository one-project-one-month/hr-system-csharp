using HRSystem.Csharp.Domain.Models.MenuGroup;

namespace HRSystem.Csharp.Domain.Features.MenuGroup;

public class DA_MenuGroup
{
    private readonly AppDbContext _context;

    public DA_MenuGroup(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<MenuGroupModel>> GetAllMenuGroups()
    {
        return await _context.TblMenuGroups
            .Where(mg => mg.DeleteFlag == false)
            .Select(mg => new MenuGroupModel
            {
                MenuGroupId = mg.MenuGroupId,
                MenuGroupCode = mg.MenuGroupCode,
                MenuGroupName = mg.MenuGroupName,
                Url = mg.Url,
                Icon = mg.Icon,
                SortOrder = mg.SortOrder,
                HasMenuItem = mg.HasMenuItem,
                CreatedAt = mg.CreatedAt,
                CreatedBy = mg.CreatedBy,
                ModifiedAt = mg.ModifiedAt,
                ModifiedBy = mg.ModifiedBy,
            })
            .ToListAsync();
    }

    public async Task<MenuGroupModel?> GetMenuGroupById(string menuGroupCode)
    {
        return await _context.TblMenuGroups
            .Where(mg => mg.MenuGroupCode.Equals(menuGroupCode) && mg.DeleteFlag == false)
            .Select(mg => new MenuGroupModel
            {
                MenuGroupId = mg.MenuGroupId,
                MenuGroupCode = mg.MenuGroupCode,
                MenuGroupName = mg.MenuGroupName,
                Url = mg.Url,
                Icon = mg.Icon,
                SortOrder = mg.SortOrder,
                HasMenuItem = mg.HasMenuItem,
                CreatedAt = mg.CreatedAt,
                CreatedBy = mg.CreatedBy,
                ModifiedAt = mg.ModifiedAt,
                ModifiedBy = mg.ModifiedBy,
            })
            .SingleOrDefaultAsync();
    }

    public async Task<Result<bool>> UpdateMenuGroup(string menuGroupCode, MenuGroupUpdateRequestModel menuGroup)
    {
        try
        {
            var existingMenuGroup = await _context.TblMenuGroups
                .Where(mg => mg.MenuGroupCode.Equals(menuGroupCode) && mg.DeleteFlag == false)
                .SingleOrDefaultAsync();

            if (existingMenuGroup == null)
            {
                return Result<bool>.Error("Menu group not found.");
            }

            var duplicate = await _context.TblMenuGroups
                .AnyAsync(mg =>
                    mg.MenuGroupName == menuGroup.MenuGroupName
                    && menuGroupCode != mg.MenuGroupCode
                    && mg.DeleteFlag == false);

            if (duplicate)
            {
                return Result<bool>.DuplicateRecordError("Menu Group Name already exists!");
            }

            existingMenuGroup.MenuGroupName = menuGroup.MenuGroupName;
            existingMenuGroup.Url = menuGroup.Url;
            existingMenuGroup.Icon = menuGroup.Icon;
            existingMenuGroup.SortOrder = menuGroup.SortOrder;
            existingMenuGroup.HasMenuItem = menuGroup.HasMenuItem;
            existingMenuGroup.ModifiedAt = DateTime.UtcNow;
            //existingMenuGroup.ModifiedBy = logginedUser;
            _context.Update(existingMenuGroup);
            var count = await _context.SaveChangesAsync();
            return count > 0
                ? Result<bool>.Success(true)
                : Result<bool>.Error("Error occurred while updating MenuGroup");
        }
        catch (Exception ex)
        {
            return Result<bool>.Error($"An error occured while updating MenuGroup: ${ex.Message}");
        }
    }

    public async Task<Result<TblMenuGroup>> CreateMenuGroupAsync(MenuGroupRequestModel requestMenuGroup)
    {
        try
        {
            var foundMenuGroup = await _context.TblMenuGroups.FirstOrDefaultAsync(
                x => x.MenuGroupCode == requestMenuGroup.MenuGroupCode
            );

            if (foundMenuGroup != null)
            {
                return Result<TblMenuGroup>.DuplicateRecordError("MenuGroup Code is already existed", foundMenuGroup);
            }

            // create new
            var menuGroup = new TblMenuGroup
            {
                MenuGroupId = Ulid.NewUlid().ToString(),
                MenuGroupCode = requestMenuGroup.MenuGroupCode,
                MenuGroupName = requestMenuGroup.MenuGroupName,
                Url = requestMenuGroup.Url,
                Icon = requestMenuGroup.Icon,
                SortOrder = requestMenuGroup.SortOrder,
                HasMenuItem = requestMenuGroup.HasMenuItem,
                CreatedAt = DateTime.UtcNow,
                //CreatedBy = loggedIn User,
                DeleteFlag = false,
            };
            await _context.AddAsync(menuGroup);
            var result = await _context.SaveChangesAsync();

            if (result <= 0)
            {
                return Result<TblMenuGroup>.Error($"Failed to create Menu Group");
            }

            return Result<TblMenuGroup>.Success(menuGroup);
        }
        catch (Exception ex)
        {
            return Result<TblMenuGroup>.Error($"An error occured while creating MenuGroup: ${ex.Message}");
        }
    }

    public async Task<Result<TblMenuGroup>> DeleteMenuGroup(string menuGroupCode)
    {
        try
        {
            var foundMenuGroup = await _context.TblMenuGroups.FirstOrDefaultAsync(
                x => x.MenuGroupCode == menuGroupCode && x.DeleteFlag == false
            );
            if (foundMenuGroup is null)
            {
                return Result<TblMenuGroup>.NotFoundError();
            }

            foundMenuGroup.DeleteFlag = true;
            foundMenuGroup.ModifiedAt = DateTime.UtcNow;

            // update its menu
            var menus = await _context.TblMenus
                .Where(x => x.MenuGroupCode == menuGroupCode)
                .ToListAsync();

            if (menus.Count <= 0 || menus is null)
            {
                return Result<TblMenuGroup>.Success(foundMenuGroup, "There aren't any menus for that menu group code.");
            }

            foreach (var menu in menus)
            {
                menu.DeleteFlag = true;
                menu.ModifiedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return Result<TblMenuGroup>.Success(foundMenuGroup);
        }
        catch (Exception ex)
        {
            {
                return Result<TblMenuGroup>.Error($"An error occured while deleting menugroups: {ex.Message}");
            }
        }
    }
}