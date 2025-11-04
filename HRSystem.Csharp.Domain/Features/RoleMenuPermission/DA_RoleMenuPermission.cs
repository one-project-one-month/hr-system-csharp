using HRSystem.Csharp.Domain.Features.Role;
using HRSystem.Csharp.Domain.Features.Sequence;
using HRSystem.Csharp.Domain.Models.RoleMenuPermission;
using HRSystem.Csharp.Shared.Enums;
using Microsoft.Extensions.Logging;

namespace HRSystem.Csharp.Domain.Features.RoleMenuPermission;

public class DA_RoleMenuPermission
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<DA_RoleMenuPermission> _logger;
    private readonly DA_Role _daRole;
    private readonly DA_Sequence _daSequence;

    public DA_RoleMenuPermission(AppDbContext dbContext,
        ILogger<DA_RoleMenuPermission> logger,
        DA_Role daRole, DA_Sequence daSequence)
    {
        _dbContext = dbContext;
        _logger = logger;
        _daRole = daRole;
        _daSequence = daSequence;
    }

    public async Task<Result<MenuTreeResponseModel>> GetMenuTreeWithPermissionsAsync(
        MenuTreeRequestModel reqModel)
    {
        try
        {
            var menuGroups = await _dbContext.TblMenuGroups
                .Where(g => !g.DeleteFlag)
                .OrderBy(g => g.SortOrder)
                .ToListAsync();

            var menuItems = await _dbContext.TblMenus
                .Where(m => !m.DeleteFlag)
                .OrderBy(m => m.SortOrder)
                .ToListAsync();

            var grantedPermissions = string.IsNullOrEmpty(reqModel.RoleCode)
                ? new List<TblRoleAndMenuPermission>()
                : await _dbContext.TblRoleAndMenuPermissions
                    .Where(p => p.RoleCode == reqModel.RoleCode && !p.DeleteFlag)
                    .ToListAsync();

            var tree = menuGroups.Select(group => new MenuGroupResponseModel
            {
                MenuGroupCode = group.MenuGroupCode,
                MenuGroupName = group.MenuGroupName,
                MenuGroupIcon = group.Icon,
                MenuGroupUrl = group.Url,
                IsChecked = !string.IsNullOrEmpty(reqModel.RoleCode) &&
                            grantedPermissions.Any(p =>
                                p.MenuGroupCode == group.MenuGroupCode && string.IsNullOrEmpty(p.MenuCode)),

                ChildMenus = menuItems
                    .Where(m => m.MenuGroupCode == group.MenuGroupCode)
                    .Select(menu => new MenuItemResponseModel
                    {
                        MenuItemCode = menu.MenuCode,
                        MenuItemName = menu.MenuName,
                        MenuItemIcon = menu.Icon,
                        MenuItemUrl = menu.Url,
                        IsChecked = !string.IsNullOrEmpty(reqModel.RoleCode) &&
                                    grantedPermissions.Any(p => p.MenuCode == menu.MenuCode)
                    }).ToList()
            }).ToList();

            var response = new MenuTreeResponseModel
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

    public async Task<Result<CreateRoleMenuPermissionResponseModel>> SaveRoleMenuPermissionsAsync(
        CreateRoleMenuPermissionRequestModel reqModel)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            var generatedCode = await _daSequence.GenerateCodeAsync(EnumSequenceCode.RL.ToString());
            var existing = await _dbContext.TblRoleAndMenuPermissions
                .Where(p => p.RoleCode == reqModel.RoleCode && !p.DeleteFlag)
                .ToListAsync();

            foreach (var item in existing)
            {
                item.DeleteFlag = true;
                item.ModifiedAt = DateTime.UtcNow;
                item.ModifiedBy = "admin";
            }

            var newPermissions = reqModel.MenuPermissions
                .Where(p => p.IsChecked)
                .Select(p => new TblRoleAndMenuPermission
                {
                    RoleAndMenuPermissionId = Guid.NewGuid().ToString(),
                    RoleAndMenuPermissionCode = generatedCode,
                    RoleCode = reqModel.RoleCode,
                    MenuGroupCode = p.MenuGroupCode,
                    MenuCode = p.MenuItemCode,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "admin",
                    DeleteFlag = false
                }).ToList();

            await _dbContext.TblRoleAndMenuPermissions.AddRangeAsync(newPermissions);
            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            var permissions = newPermissions
                .Select(p => new CreateRoleMenuPermissionModel
                {
                    RoleAndMenuPermissionId = p.RoleAndMenuPermissionId,
                    RoleAndMenuPermissionCode = p.RoleAndMenuPermissionCode,
                    RoleCode = p.RoleCode,
                    MenuGroupCode = p.MenuGroupCode,
                    MenuCode = p.MenuCode,
                    CreatedDateTime = p.CreatedAt,
                    CreatedUserId = p.CreatedBy
                }).ToList();

            var response = new CreateRoleMenuPermissionResponseModel()
            {
                RoleMenuPermissions = permissions
            };

            return Result<CreateRoleMenuPermissionResponseModel>.Success(response);
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            return Result<CreateRoleMenuPermissionResponseModel>
                .SystemError("Failed to create role menu permissions for role - {}");
        }
    }
}