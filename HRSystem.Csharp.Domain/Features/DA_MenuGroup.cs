using HRSystem.Csharp.Domain.Models;
using HRSystem.Csharp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;



namespace HRSystem.Csharp.Domain.Features
{
    public class DA_MenuGroup
    {
        private readonly AppDbContext _appDbContext;

        public DA_MenuGroup (AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }


        public async Task<Result<TblMenuGroup>> CreateMenuGroupAsync(MenuGroupRequestModel requestMenuGroup)
        {
            try
            {
                var foundMenuGroup = await _appDbContext.TblMenuGroups.FirstOrDefaultAsync(
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
                    HasMenuGroup = requestMenuGroup.HasMenuGroup,
                    CreatedAt = DateTime.UtcNow,
                    //CreatedBy = requestMenuGroup.CreatedBy,
                    DeleteFlag = false,
                };
                await _appDbContext.AddAsync(menuGroup);
                var result = await _appDbContext.SaveChangesAsync();

                if (result <= 0)
                {
                    return Result<TblMenuGroup>.Error($"Failed to create Menu Group");
                }
                return Result<TblMenuGroup>.Success(menuGroup);
            }catch(Exception ex)
            {
                return Result<TblMenuGroup>.Error($"An error occured while creating MenuGroup: ${ex.Message}");
            }

        }

        public async Task<Result<TblMenuGroup>> DeleteMenuGroup(string menuGroupCode)
        {
            try
            {
                var foundMenuGroup = await _appDbContext.TblMenuGroups.FirstOrDefaultAsync(
                    x => x.MenuGroupCode == menuGroupCode && x.DeleteFlag == false
                    );
                if (foundMenuGroup is null)
                {
                    return Result<TblMenuGroup>.NotFoundError();
                }

                foundMenuGroup.DeleteFlag = true;
                foundMenuGroup.ModifiedAt = DateTime.UtcNow;
                
                // update its menu
                var menus = await _appDbContext.TblMenus
                    .Where(x => x.MenuGroupCode == menuGroupCode)
                    .ToListAsync();

                if (menus.Count <= 0 || menus is null)
                {
                    return Result<TblMenuGroup>.Success(foundMenuGroup,"There aren't any menus for that menu group code.");
                }

                foreach (var menu in menus)
                {
                    menu.DeleteFlag = true;
                    menu.ModifiedAt = DateTime.UtcNow;
                }

                await _appDbContext.SaveChangesAsync();
                return Result<TblMenuGroup>.Success(foundMenuGroup);
            }
            catch (Exception ex)
            {
                {
                    return Result<TblMenuGroup>.Error($"An error occured while retrieving roles: {ex.Message}");
                }
            }

        }
    }
}
