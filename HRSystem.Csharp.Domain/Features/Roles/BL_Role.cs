using HRSystem.Csharp.Domain.Models.Roles;

namespace HRSystem.Csharp.Domain.Features.Roles;

public class BL_Role
{
    private readonly DA_Role _daRole;

    public BL_Role(DA_Role daRole)
    {
        _daRole = daRole;
    }

    public async Task<Result<List<RoleResponseModel>>> GetAllRoles()
    {
        var roles = await _daRole.GetAllRoles();
        var response = Result<List<RoleResponseModel>>.Success(roles.Data);
        return response;
    }

    public async Task<Result<bool>> CreateRole(RoleRequestModel role)
    {
        if (role == null)
        {
            return Result<bool>.BadRequestError("New role cannot be empty");
        }
        var response = await _daRole.CreateRole(role);
        return response;
    }

    public async Task<Result<RoleResponseModel>> GetRoleByCode(string roleCode)
    {
        if (string.IsNullOrEmpty(roleCode))
        {
            return Result<RoleResponseModel>.BadRequestError("Role code cannot be empty");
        }
        var response = await _daRole.GetRoleByCode(roleCode);
        return response;
    }

    public async Task<Result<bool>> UpdateRole(RoleUpdateRequestModel role, string roleCode)
    {
        if (string.IsNullOrEmpty(roleCode))
        {
            return Result<bool>.BadRequestError("Role code cannot be empty");
        }
        else if (role == null)
        {
            return Result<bool>.BadRequestError("Updated role cannot be empty");
        }
        var response = await _daRole.UpdateRole(role, roleCode);
        return response;
    }

    public async Task<Result<bool>> DeleteRole(string roleCode)
    {
        if (string.IsNullOrEmpty(roleCode))
        {
            return Result<bool>.BadRequestError("Role code cannot be empty");

        }
        var response = await _daRole.DeleteRole(roleCode);
        return response;
    }
}