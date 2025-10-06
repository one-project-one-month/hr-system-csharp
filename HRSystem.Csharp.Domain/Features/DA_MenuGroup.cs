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
    public class DA_MenuGroup
    {
        private readonly AppDbContext _context;
        public DA_MenuGroup(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<MenuGroup>> GetAllMenuGroups()
        {  return await _context.TblMenuGroups
                .Where(mg => mg.DeleteFlag == false)
                .Select(mg => new MenuGroup
                {
                    MenuGroupId = mg.MenuGroupId,
                    MenuGroupCode = mg.MenuGroupCode,
                    MenuGroupName = mg.MenuGroupName,
                    Url = mg.Url,
                    Icon = mg.Icon,
                    SortOrder = mg.SortOrder,
                    HasMenuGroup = mg.HasMenuGroup,
                    CreatedAt = mg.CreatedAt,
                    CreatedBy = mg.CreatedBy,
                    ModifiedAt = mg.ModifiedAt,
                    ModifiedBy = mg.ModifiedBy,
                })
                .ToListAsync();
        }

        public async Task<MenuGroup?> GetMenuGroupById(string menuGroupId)
        {
            return await _context.TblMenuGroups
                .Where(mg => mg.MenuGroupId.Equals(menuGroupId) && mg.DeleteFlag == false)
                .Select(mg => new MenuGroup
                {
                    MenuGroupId = mg.MenuGroupId,
                    MenuGroupCode = mg.MenuGroupCode,
                    MenuGroupName = mg.MenuGroupName,
                    Url = mg.Url,
                    Icon = mg.Icon,
                    SortOrder = mg.SortOrder,
                    HasMenuGroup = mg.HasMenuGroup,
                    CreatedAt = mg.CreatedAt,
                    CreatedBy = mg.CreatedBy,
                    ModifiedAt = mg.ModifiedAt,
                    ModifiedBy = mg.ModifiedBy,
                })
                .SingleOrDefaultAsync();
            
        }
   
        public async Task<Result<MenuGroup>> CreateMenuGroup(MenuGroupRequestModel menuGroup)
        {
            var newMenuGroup = new TblMenuGroup
            {
                MenuGroupId = Ulid.NewUlid().ToString(),
                MenuGroupCode = menuGroup.MenuGroupCode,
                MenuGroupName = menuGroup.MenuGroupName,
                Url = menuGroup.Url,
                Icon = menuGroup.Icon,
                SortOrder = menuGroup.SortOrder,
                HasMenuGroup = menuGroup.HasMenuGroup,
                CreatedAt = DateTime.UtcNow,
                //CreatedBy = menuGroup.CreatedBy,
                DeleteFlag = false
            };
            _context.TblMenuGroups.Add(newMenuGroup);
            await _context.SaveChangesAsync();
            var createdMenuGroup = new MenuGroup
            {
                MenuGroupId = newMenuGroup.MenuGroupId,
                MenuGroupCode = newMenuGroup.MenuGroupCode,
                MenuGroupName = newMenuGroup.MenuGroupName,
                Url = newMenuGroup.Url,
                Icon = newMenuGroup.Icon,
                SortOrder = newMenuGroup.SortOrder,
                HasMenuGroup = newMenuGroup.HasMenuGroup,
                CreatedAt = newMenuGroup.CreatedAt,
                CreatedBy = newMenuGroup.CreatedBy
            };
            return Result<MenuGroup>.Success(createdMenuGroup);
        }
    
        public async Task<Result<bool>> UpdateMenuGroup(string menuGroupId, MenuGroupRequestModel menuGroup)
        {
            var existingMenuGroup = await _context.TblMenuGroups
                .Where(mg => mg.MenuGroupId.Equals(menuGroupId) && mg.DeleteFlag == false)
                .SingleOrDefaultAsync();
            if (existingMenuGroup == null)
            {
                return Result<bool>.Error("Menu group not found.");
            }
            existingMenuGroup.MenuGroupCode = menuGroup.MenuGroupCode;
            existingMenuGroup.MenuGroupName = menuGroup.MenuGroupName;
            existingMenuGroup.Url = menuGroup.Url;  
            existingMenuGroup.Icon = menuGroup.Icon;
            existingMenuGroup.SortOrder = menuGroup.SortOrder;
            existingMenuGroup.HasMenuGroup = menuGroup.HasMenuGroup;
            existingMenuGroup.ModifiedAt = DateTime.UtcNow;
            //existingMenuGroup.ModifiedBy = menuGroup.ModifiedBy;
            _context.Update(existingMenuGroup);
            await _context.SaveChangesAsync();
            return Result<bool>.Success(true);
        }
    
        public async Task<Result<bool>> DeleteMenuGroup(string menuGroupId)
        {
            var existingMenuGroup = await _context.TblMenuGroups
                .Where(mg => mg.MenuGroupId.Equals(menuGroupId) && mg.DeleteFlag == false)
                .SingleOrDefaultAsync();
            if (existingMenuGroup == null)
            {
                return Result<bool>.Error("Menu group not found.");
            }
            existingMenuGroup.DeleteFlag = true;
            existingMenuGroup.ModifiedAt = DateTime.UtcNow;
            //existingMenuGroup.ModifiedBy = modifiedBy;
            _context.Update(existingMenuGroup);
            await _context.SaveChangesAsync();
            return Result<bool>.Success(true);
        }
    }
}
