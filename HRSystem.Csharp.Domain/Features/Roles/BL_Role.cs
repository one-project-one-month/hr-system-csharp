using HRSystem.Csharp.Domain.Models.Roles;
using Microsoft.Extensions.Logging;

namespace HRSystem.Csharp.Domain.Features.Roles;

public class BL_Role
{
    private readonly DA_Role _daRole;
    private readonly ILogger<BL_Role> _logger;

    public BL_Role(DA_Role daRole, ILogger<BL_Role> logger)
    {
        _daRole = daRole;
        _logger = logger;
    }

    public async Task<Result<RoleListResponseModel>> GetAllRoles(RoleListRequestModel reqModel)
    {
        if (reqModel == null)
        {
            _logger.LogWarning("Role list request is null");
            return Result<RoleListResponseModel>.BadRequestError("Request cannot be null");
        }

        return await _daRole.GetRoles(reqModel);
    }

    public async Task<Result<bool>> CreateRole(RoleRequestModel role)
    {
        if (role == null || string.IsNullOrWhiteSpace(role.RoleName))
        {
            return Result<bool>.InvalidDataError("Role name is required!");
        }

        var existing = await _daRole.GetByRoleName(role.RoleName);

        if (existing.IsNotFound)
        {
            return Result<bool>.Error(existing.Message);
        }

        if (existing.IsSuccess)
        {
            return Result<bool>.DuplicateRecordError("Role name already exists!");
        }

        var newRole = new RoleCreateRequestModel
        {
            RoleId = Ulid.NewUlid().ToString(),
            RoleName = role.RoleName,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "admin"
        };

        return await _daRole.CreateRole(newRole);
    }

    public async Task<Result<RoleResponseModel>> GetRoleByCode(RoleEditRequestModel reqModel)
    {
        if (string.IsNullOrWhiteSpace(reqModel.RoleCode))
        {
            return Result<RoleResponseModel>.BadRequestError("Role code cannot be empty");
        }

        var roleResult = await _daRole.GetByRoleCode(reqModel.RoleCode);
        if (roleResult.IsError)
        {
            return Result<RoleResponseModel>.Error(roleResult.Message);
        }

        var role = roleResult.Data!;
        var response = new RoleResponseModel
        {
            RoleName = role.RoleName,
            RoleCode = role.RoleCode,
            RoleId = role.RoleId,
            CreatedAt = role.CreatedAt,
            CreatedBy = role.CreatedBy,
            ModifiedAt = role.ModifiedAt,
            ModifiedBy = role.ModifiedBy,
            DeleteFlag = role.DeleteFlag
        };

        return Result<RoleResponseModel>.Success(response);
    }

    public async Task<Result<bool>> UpdateRole(RoleUpdateRequestModel reqModel)
    {
        if (reqModel == null || string.IsNullOrWhiteSpace(reqModel.RoleCode) ||
            string.IsNullOrWhiteSpace(reqModel.RoleName))
        {
            return Result<bool>.InvalidDataError("Role code and name are required");
        }

        var existing = await _daRole.GetByRoleCode(reqModel.RoleCode);
        if (!existing.IsSuccess)
        {
            return Result<bool>.Error(existing.Message);
        }

        var role = existing.Data!;
        role.RoleName = reqModel.RoleName;
        role.ModifiedAt = DateTime.UtcNow;
        role.ModifiedBy = "admin";

        return await _daRole.UpdateRole(role);
    }

    public async Task<Result<bool>> DeleteRole(RoleDeleteRequestModel reqModel)
    {
        if (string.IsNullOrWhiteSpace(reqModel.RoleCode))
        {
            return Result<bool>.BadRequestError("Role code cannot be empty");
        }

        var roleResult = await _daRole.GetByRoleCode(reqModel.RoleCode);
        if (!roleResult.IsSuccess)
        {
            return Result<bool>.Error(roleResult.Message);
        }

        return await _daRole.DeleteRole(roleResult.Data!);
    }
}