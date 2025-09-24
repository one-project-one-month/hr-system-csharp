using HRSystem.Csharp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRSystem.Csharp.Domain.Models;

namespace HRSystem.Csharp.Domain.Features
{
    public class DA_Role
    {
        private readonly AppDbContext _appDbContext;

        public DA_Role(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public Result<List<Role>> GetAllRoles()
        {
            try
            {
                var roles = _appDbContext.TblRoles
                    .Select(r => new Role
                    {
                        Name = r.RoleName
                    })
                    .ToList();
                return Result<List<Role>>.Success(roles);
            }
            catch (Exception ex)
            {
                return Result<List<Role>>.Error($"An error occurred while retrieving roles: {ex.Message}");
            }
        }
    }

    
}
