using HRSystem.Csharp.Domain.Models.RoleMenuPermission;
using Microsoft.Extensions.Logging;

namespace HRSystem.Csharp.Domain.Features.RoleMenuPermission;

public class DA_RoleMenuPermission
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<DA_RoleMenuPermission> _logger;

    public DA_RoleMenuPermission(AppDbContext dbContext, ILogger<DA_RoleMenuPermission> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<MenuTreeResponseModel>> GetMenuTreeAsync()
    {
        try
        {
            var groups = await _dbContext.TblMenuGroups
                .Where(g => !g.DeleteFlag)
                .OrderBy(g => g.SortOrder)
                .ToListAsync();

            var menus = await _dbContext.TblMenus
                .Where(m => !m.DeleteFlag)
                .OrderBy(m => m.SortOrder)
                .ToListAsync();

            var tree = groups.Select(g => new MenuGroupResponseModel()
            {
                GroupCode = g.MenuGroupCode,
                GroupName = g.MenuGroupName,
                GroupIcon = g.Icon,
                GroupUrl = g.Url,
                Items = menus
                    .Where(m => m.MenuGroupCode == g.MenuGroupCode)
                    .Select(m => new MenuItemResponseModel()
                    {
                        MenuCode = m.MenuCode,
                        MenuName = m.MenuName,
                        MenuIcon = m.Icon,
                        MenuUrl = m.Url
                    }).ToList()
            }).ToList();

            var response = new MenuTreeResponseModel()
            {
                MenuTree = tree
            };

            return Result<MenuTreeResponseModel>.Success(response);
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
            return Result<MenuTreeResponseModel>.Error("Failed to retrieve menu tree from database.");
        }
    }
}