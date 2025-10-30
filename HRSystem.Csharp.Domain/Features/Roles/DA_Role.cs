using HRSystem.Csharp.Domain.Models.Roles;

namespace HRSystem.Csharp.Domain.Features.Roles;

public class DA_Role
{
    private readonly AppDbContext _appDbContext;

    public DA_Role(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<Result<List<RoleResponseModel>>> GetAllRoles()
    {
        try
        {
            var roles = _appDbContext.TblRoles
                .AsNoTracking()
                .Where(r => r.DeleteFlag != true)
                .Select(r => new RoleResponseModel
                {
                    RoleName = r.RoleName,
                    RoleCode = r.RoleCode,
                    RoleId = r.RoleId,
                    UniqueName = r.UniqueName,
                    CreatedAt = r.CreatedAt,
                    CreatedBy = r.CreatedBy,
                    ModifiedAt = r.ModifiedAt,
                    ModifiedBy = r.ModifiedBy,
                    DeleteFlag = r.DeleteFlag,

                })
                .ToListAsync();
            return Result<List<RoleResponseModel>>.Success(await roles);
        }
        catch (Exception ex)
        {
            return Result<List<RoleResponseModel>>.Error($"An error occurred while retrieving roles: {ex.Message}");
        }
    }

    public async Task<Result<bool>> CreateRole(RoleRequestModel role)
    {
        try
        {
            var existing = _appDbContext.TblRoles.FirstOrDefault(r => r.RoleCode == role.RoleCode && r.DeleteFlag != true);
            if (existing != null)
            {
                return Result<bool>.InvalidDataError("A duplicate role has already been created!");
            }

            TblRole newRole = new TblRole()
            {
                RoleId = Ulid.NewUlid().ToString(),
                RoleName = role.RoleName,
                RoleCode = role.RoleCode,
                UniqueName = role.UniqueName,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "admin",
                ModifiedAt = null,
                ModifiedBy = null,
                DeleteFlag = false
            };
            await _appDbContext.TblRoles.AddAsync(newRole);
            var result = await _appDbContext.SaveChangesAsync();
            return result > 0 ? Result<bool>.Success(true)
                    : Result<bool>.Error();
        }
        catch (Exception ex)
        {
            return Result<bool>.Error($"An error occurred while creating role: {ex.Message}");
        }
    }

    public async Task<Result<RoleResponseModel>> GetRoleByCode(string roleCode)
    {

        try
        {
            var role = await _appDbContext.TblRoles.Where(r => r.DeleteFlag != true)
                .Select(r => new RoleResponseModel()
                {
                    RoleName = r.RoleName,
                    RoleCode = r.RoleCode,
                    RoleId = r.RoleId,
                    UniqueName = r.UniqueName,
                    CreatedAt = r.CreatedAt,
                    CreatedBy = r.CreatedBy,
                    ModifiedAt = r.ModifiedAt,
                    ModifiedBy = r.ModifiedBy,
                    DeleteFlag = r.DeleteFlag
                }).FirstOrDefaultAsync(r => r.RoleCode == roleCode);

            if (role == null) return Result<RoleResponseModel>.NotFoundError("Cannot find role with given code");

            return Result<RoleResponseModel>.Success(role);
        }
        catch (Exception ex)
        {
            return Result<RoleResponseModel>.Error($"An error occurred while retrieving roles: {ex.Message}");
        }
    }

    public async Task<Result<bool>> UpdateRole(RoleUpdateRequestModel role, string roleCode)
    {
        var existingRole = await _appDbContext.TblRoles.FirstOrDefaultAsync(r => r.RoleCode == roleCode && r.DeleteFlag != true);
        if (existingRole is null) return Result<bool>.NotFoundError("Cannot find the role to be updated");
        if (role is null) return Result<bool>.InvalidDataError("Data to update role cannot be empty");

        existingRole.RoleName = role.RoleName;
        existingRole.UniqueName = role.UniqueName;
        existingRole.ModifiedAt = DateTime.UtcNow;
        existingRole.ModifiedBy = "admin";

        var result = await _appDbContext.SaveChangesAsync();
        return result > 0 ? Result<bool>.Success(true)
                : Result<bool>.Error();
    }

    public async Task<Result<bool>> DeleteRole(string roleCode)
    {
        var roleToBeDeleted = await _appDbContext.TblRoles.FirstOrDefaultAsync(r => r.RoleCode == roleCode && r.DeleteFlag != true);
        if (roleToBeDeleted is null) return Result<bool>.NotFoundError("Cannot find the role to be deleted");

        roleToBeDeleted.DeleteFlag = true;
        var result = _appDbContext.SaveChanges();
        return result > 0 ? Result<bool>.Success(true)
                : Result<bool>.Error();
    }
}