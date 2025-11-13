using HRSystem.Csharp.Domain.Models.RoleMenuPermission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Csharp.Domain.Features.RoleMenuPermission
{
    public class DA_Permission
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<DA_RoleMenuPermission> _logger;

        public DA_Permission(AppDbContext dbContext, ILogger<DA_RoleMenuPermission> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Result<PermissionModel>> GetPermissionByCode(string permissionCode)
        {
            var permission = await _dbContext.TblPermissions.SingleOrDefaultAsync(p => p.PermissionCode == permissionCode);
            if (permission is null)
                return Result<PermissionModel>.NotFoundError($"Permission with the {permissionCode} not found!");

            var responseModel = new PermissionModel
            {
                PermissionId = permission.PermissionId,
                PermissionName = permission.PermissionName,
                PermissionCode = permission.PermissionCode
            };
            return Result<PermissionModel>.Success(responseModel);
        }
    }
}
