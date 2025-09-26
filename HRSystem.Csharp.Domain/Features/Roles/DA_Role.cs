using HRSystem.Csharp.Domain.Models;
using HRSystem.Csharp.Domain.Models.Roles;
using HRSystem.Csharp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Csharp.Domain.Features.Roles
{
    public class DA_Role
    {
        
        private readonly AppDbContext _appDbContext;

        public DA_Role(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public Result<List<RoleResponseModel>> GetAllRoles()
        {
            try
            {
                var roles = _appDbContext.TblRoles
                    .Where(r => r.DeleteFlag != true)
                    .Select(r => new RoleResponseModel
                    {
                        RoleId = r.RoleId,
                        RoleName = r.RoleName != null ? r.RoleName : "",
                        RoleCode = r.RoleCode != null ? r.RoleCode : "",
                        UniqueName = r.UniqueName != null ? r.UniqueName : "",
                        CreatedAt = r.CreatedAt,
                        CreatedBy = r.CreatedBy,
                        ModifiedAt = r.ModifiedAt,
                        ModifiedBy = r.ModifiedBy,
                        DeleteFlag = r.DeleteFlag,
                    })
                    .ToList();
                return Result<List<RoleResponseModel>>.Success(roles);
            }
            catch (Exception ex)
            {
                return Result<List<RoleResponseModel>>.Error($"An error occurred while retrieving roles: {ex.Message}");
            }
        }

        public Result<int> CreateRole(RoleRequestModel role)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(role.RoleCode) || string.IsNullOrWhiteSpace(role.RoleName) || string.IsNullOrWhiteSpace(role.UniqueName))
                {
                    return Result<int>.ValidationError("RoleCode, RoleName, and UniqueName are required.");
                }

                bool isExists = _appDbContext.TblRoles.Any(r => (r.RoleCode == role.RoleCode || r.UniqueName == role.UniqueName) && (r.DeleteFlag != true));

                if (isExists)
                    return Result<int>.DuplicateRecordError("A role with the same RoleCode or UniqueName already exists.");

                var entity = new TblRole
                {
                    RoleId = Guid.NewGuid().ToString(),
                    RoleCode = role.RoleCode,
                    RoleName = role.RoleName,
                    UniqueName = role.UniqueName,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "admin", // Admin for now 
                    DeleteFlag = false
                };

                _appDbContext.TblRoles.Add(entity);
                int result = _appDbContext.SaveChanges();

                return Result<int>.Success(result, "Role created successfully.");



            }
            catch (Exception ex)
            {
                return Result<int>.Error($"An error occurred while retrieving roles: {ex.Message}");
            }
        }

        // Use unique name or roleId or roleCode ? 
        public Result<RoleResponseModel> EditRole(string roleCode)
        {
            try
            {
                var existing = _appDbContext.TblRoles.FirstOrDefault(r => r.RoleCode == roleCode && r.DeleteFlag != true);
                if (existing == null)
                    return Result<RoleResponseModel>.NotFoundError("A role with the same Id doesn't exist.");

                var role = new RoleResponseModel
                {
                    RoleId = existing.RoleId,
                    RoleName = existing.RoleName != null ? existing.RoleName : "",
                    RoleCode = existing.RoleCode != null ? existing.RoleCode : "",
                    UniqueName = existing.UniqueName != null ? existing.UniqueName : "",
                    CreatedAt = existing.CreatedAt,
                    CreatedBy = existing.CreatedBy,
                    ModifiedAt = existing.ModifiedAt,
                    ModifiedBy = existing.ModifiedBy,

                };

                return Result<RoleResponseModel>.Success(role, "Role found successfully");

            }
            catch (Exception ex)
            {
                return Result<RoleResponseModel>.Error($"An error occurred while retrieving role: {ex.Message}");

            }
        }

        public Result<int> UpdateRole(RoleUpdateRequestModel updatedRole, string roleCode)
        {
            try
            {
                if (updatedRole == null)
                {
                    return Result<int>.InvalidDataError("Updated role cannot be null");
                }
                var existing = _appDbContext.TblRoles.FirstOrDefault(r => r.RoleCode == roleCode && r.DeleteFlag != true);
                if (existing == null)
                {
                    return Result<int>.NotFoundError("Cannot find role to be updated");

                }
                existing.RoleName = updatedRole.RoleName;
                existing.UniqueName = updatedRole.UniqueName;
                existing.ModifiedAt = DateTime.UtcNow;
                existing.ModifiedBy = "admin";
                var result = _appDbContext.SaveChanges();
                return Result<int>.Success(result, "Role Updated Successfully");
            }
            catch (Exception ex)
            {
                return Result<int>.Error($"An error occurred while updating role: {ex.Message}");


            }

        }

        public Result<int> DeleteRole(string roleCode)
        {
            try
            {
                if (string.IsNullOrEmpty(roleCode))
                {
                    return Result<int>.InvalidDataError("Role code cannot be empty");
                }
                var existing = _appDbContext.TblRoles.FirstOrDefault(r => r.RoleCode == roleCode && r.DeleteFlag != true);
                if (existing == null)
                {
                    return Result<int>.NotFoundError("Cannot find role to delete");
                }
                existing.DeleteFlag = true;
                var result = _appDbContext.SaveChanges();
                return Result<int>.Success(result, "Role Deleted Successfully");

            } catch(Exception ex)
            {
                return Result<int>.Error($"An error occurred while deleting role: {ex.Message}");

            }
        }

        
    }
        
}
