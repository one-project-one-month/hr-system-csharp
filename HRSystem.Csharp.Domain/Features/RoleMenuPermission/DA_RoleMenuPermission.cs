namespace HRSystem.Csharp.Domain.Features.RoleMenuPermission;

public class DA_RoleMenuPermission
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<DA_RoleMenuPermission> _logger;
    private readonly DA_Sequence _daSequence;

    public DA_RoleMenuPermission(AppDbContext dbContext,
        ILogger<DA_RoleMenuPermission> logger,
        DA_Sequence daSequence)
    {
        _dbContext = dbContext;
        _logger = logger;
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

            var permissions = await _dbContext.TblPermissions.ToListAsync();

            var grantedPermissions = string.IsNullOrEmpty(reqModel.RoleCode)
                ? new List<TblRoleAndMenuPermission>()
                : await _dbContext.TblRoleAndMenuPermissions
                    .Where(p => p.RoleCode == reqModel.RoleCode && !p.DeleteFlag)
                    .ToListAsync();

            var tree = menuGroups.Select(group =>
            {
                // CASE 1: group has menu items
                if (group.HasMenuItem == true)
                {
                    var childMenus = menuItems
                        .Where(m => m.MenuGroupCode == group.MenuGroupCode)
                        .Select(menu =>
                        {
                            var menuPermissions = grantedPermissions
                                .Where(g => g.MenuCode == menu.MenuCode)
                                .Select(g => g.PermissionCode)
                                .Where(code => !string.IsNullOrWhiteSpace(code))
                                .Distinct()
                                .ToList();

                            return new MenuItemResponseModel
                            {
                                MenuItemCode = menu.MenuCode,
                                MenuItemName = menu.MenuName,
                                MenuItemIcon = menu.Icon,
                                MenuItemUrl = menu.Url,
                                Permissions = menuPermissions,
                                IsChecked = menuPermissions.Any()
                            };
                        })
                       .ToList();

                    return new MenuGroupResponseModel
                    {
                        MenuGroupCode = group.MenuGroupCode,
                        MenuGroupName = group.MenuGroupName,
                        MenuGroupIcon = group.Icon,
                        MenuGroupUrl = group.Url,
                        IsChecked = !string.IsNullOrEmpty(reqModel.RoleCode) &&
                                    grantedPermissions.Any(p =>
                                        p.MenuGroupCode == group.MenuGroupCode && !string.IsNullOrEmpty(p.MenuCode)),
                        ChildMenus = childMenus
                    };
                }



                // CASE 2: group has NO menu items - group-level permissions 
                var groupPermissions = grantedPermissions
                    .Where(g => g.MenuGroupCode == group.MenuGroupCode)
                    .Select(g => g.PermissionCode)
                    .Where(code => !string.IsNullOrWhiteSpace(code))
                    .Distinct()
                    .ToList();

                if (!groupPermissions.Any())
                {
                    return new MenuGroupResponseModel
                    {
                        MenuGroupCode = group.MenuGroupCode,
                        MenuGroupName = group.MenuGroupName,
                        MenuGroupIcon = group.Icon,
                        MenuGroupUrl = group.Url,
                        IsChecked = !string.IsNullOrEmpty(reqModel.RoleCode) &&
                                    grantedPermissions.Any(p =>
                                        p.MenuGroupCode == group.MenuGroupCode),
                        ChildMenus = []
                    };
                }

                return new MenuGroupResponseModel
                {
                    MenuGroupCode = group.MenuGroupCode,
                    MenuGroupName = group.MenuGroupName,
                    MenuGroupIcon = group.Icon,
                    MenuGroupUrl = group.Url,
                    IsChecked = !string.IsNullOrEmpty(reqModel.RoleCode) &&
                                    grantedPermissions.Any(p =>
                                        p.MenuGroupCode == group.MenuGroupCode &&
                                         group.HasMenuItem == false),
                    ChildMenus = new List<MenuItemResponseModel>
                        {
                            new MenuItemResponseModel
                            {
                                MenuItemCode = null,
                                MenuItemName = null,
                                MenuItemIcon = null,
                                MenuItemUrl = null,
                                IsChecked = groupPermissions.Any(),
                                Permissions = groupPermissions
                            }
                        }
                };
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
                _dbContext.TblRoleAndMenuPermissions.Update(item);
            }
            await transaction.CommitAsync();
            await _dbContext.SaveChangesAsync();

            var newPermissions = reqModel.MenuPermissions
                .Where(p => p.IsChecked)
                .Select(p => new TblRoleAndMenuPermission
                {
                    RoleAndMenuPermissionId = Guid.NewGuid().ToString(),
                    RoleAndMenuPermissionCode = generatedCode,
                    RoleCode = reqModel.RoleCode,
                    MenuGroupCode = p.MenuGroupCode,
                    MenuCode = p.MenuItemCode ?? null,
                    PermissionCode = p.PermissionCode,
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
                    RoleAndMenuPermissionCode = p.RoleAndMenuPermissionCode!,
                    RoleCode = p.RoleCode,
                    MenuGroupCode = p.MenuGroupCode!,
                    MenuCode = p.MenuCode ?? null,
                    PermissionCode = p.PermissionCode ?? null,
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
            _logger.LogError(e.ToString());
            return Result<CreateRoleMenuPermissionResponseModel>
                .SystemError("Failed to create role menu permissions for role - {}");
        }
    }
}