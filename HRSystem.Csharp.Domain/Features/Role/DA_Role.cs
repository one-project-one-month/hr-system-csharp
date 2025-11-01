using HRSystem.Csharp.Domain.Features.Sequence;
using HRSystem.Csharp.Domain.Models.Roles;
using HRSystem.Csharp.Shared.Enums;
using Microsoft.Extensions.Logging;

namespace HRSystem.Csharp.Domain.Features.Role;

public class DA_Role
{
    private readonly AppDbContext _appDbContext;
    private readonly ILogger<DA_Role> _logger;
    private readonly DA_Sequence _daSequence;

    public DA_Role(AppDbContext appDbContext, ILogger<DA_Role> logger, DA_Sequence daSequence)
    {
        _appDbContext = appDbContext;
        _logger = logger;
        _daSequence = daSequence;
    }

    public async Task<Result<RoleListResponseModel>> GetRoles(RoleListRequestModel reqModel)
    {
        try
        {
            var query = _appDbContext.TblRoles
                .AsNoTracking()
                .Where(r => !r.DeleteFlag);

            if (!string.IsNullOrWhiteSpace(reqModel.RoleName))
            {
                query = query.Where(r => r.RoleName.ToLower() == reqModel.RoleName.ToLower());
            }

            query = query.OrderByDescending(r => r.CreatedAt);

            var roles = query.Select(r => new RoleResponseModel
            {
                RoleName = r.RoleName,
                RoleCode = r.RoleCode,
                RoleId = r.RoleId,
                CreatedAt = r.CreatedAt,
                CreatedBy = r.CreatedBy,
                ModifiedAt = r.ModifiedAt,
                ModifiedBy = r.ModifiedBy,
                DeleteFlag = r.DeleteFlag
            });

            var pagedResult = await roles.GetPagedResultAsync(reqModel.PageNo, reqModel.PageSize);

            var result = new RoleListResponseModel
            {
                Items = pagedResult.Items,
                TotalCount = pagedResult.TotalCount,
                PageNo = reqModel.PageNo,
                PageSize = reqModel.PageSize
            };

            return Result<RoleListResponseModel>.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching roles");
            return Result<RoleListResponseModel>.SystemError("An error occurred while retrieving roles.");
        }
    }

    public async Task<Result<TblRole>> GetByRoleCode(string roleCode)
    {
        try
        {
            var role = await _appDbContext.TblRoles
                .FirstOrDefaultAsync(r => r.RoleCode == roleCode && !r.DeleteFlag);

            return role != null
                ? Result<TblRole>.Success(role)
                : Result<TblRole>.NotFoundError("Role not found.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching role by code");
            return Result<TblRole>.SystemError("An error occurred while retrieving the role.");
        }
    }

    public async Task<Result<TblRole>> GetByRoleName(string roleName)
    {
        try
        {
            var role = await _appDbContext.TblRoles
                .FirstOrDefaultAsync(r => r.RoleName == roleName && !r.DeleteFlag);

            return role != null
                ? Result<TblRole>.Success(role)
                : Result<TblRole>.NotFoundError("Role not found.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching role by name");
            return Result<TblRole>.SystemError("An error occurred while retrieving the role.");
        }
    }

    public async Task<Result<bool>> CreateRole(RoleCreateRequestModel reqModel)
    {
        try
        {
            var generatedCode = await _daSequence.GenerateCodeAsync(EnumSequenceCode.RL.ToString());
            var role = new TblRole
            {
                RoleId = reqModel.RoleId,
                RoleCode = generatedCode,
                RoleName = reqModel.RoleName,
                CreatedAt = reqModel.CreatedAt,
                CreatedBy = reqModel.CreatedBy,
                DeleteFlag = false
            };

            await _appDbContext.TblRoles.AddAsync(role);
            var saved = await _appDbContext.SaveChangesAsync() > 0;

            return saved
                ? Result<bool>.Success("Role created successfully!")
                : Result<bool>.Error("Failed to save role.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating role");
            return Result<bool>.SystemError("An error occurred while creating the role.");
        }
    }

    public async Task<Result<bool>> UpdateRole(TblRole role)
    {
        try
        {
            var updated = await _appDbContext.SaveChangesAsync() > 0;
            return updated
                ? Result<bool>.Success("Role updated successfully!")
                : Result<bool>.Error("Failed to update role.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating role");
            return Result<bool>.SystemError("An error occurred while updating the role.");
        }
    }

    public async Task<Result<bool>> DeleteRole(TblRole role)
    {
        try
        {
            role.DeleteFlag = true;
            var deleted = await _appDbContext.SaveChangesAsync() > 0;
            return deleted
                ? Result<bool>.Success(true)
                : Result<bool>.Error("Failed to delete role.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting role");
            return Result<bool>.SystemError("An error occurred while deleting the role.");
        }
    }
}