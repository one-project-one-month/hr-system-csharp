using HRSystem.Csharp.Domain.Models;
using HRSystem.Csharp.Domain.Models.Roles;
using HRSystem.Csharp.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Csharp.Domain.Features.Roles
{
    public class BL_Role
    {
        
        private readonly DA_Role _daRole;

        public BL_Role(DA_Role daRole)
        {
            _daRole = daRole;
        }

        public Result<List<RoleResponseModel>> GetAllRoles()
        {
            var roles = _daRole.GetAllRoles();
            var response = Result<List<RoleResponseModel>>.Success(roles.Data);
            return response;
        }

        public Result<int> CreateRole(RoleRequestModel role)
        {
            if (string.IsNullOrWhiteSpace(role.RoleCode) || string.IsNullOrWhiteSpace(role.RoleName) || string.IsNullOrWhiteSpace(role.UniqueName))
                return Result<int>.ValidationError("All fields are required.");

            return _daRole.CreateRole(role);
        }

        public Result<RoleResponseModel> EditRole(string roleCode)
        {
            if (string.IsNullOrWhiteSpace(roleCode))
            {
                return Result<RoleResponseModel>.ValidationError("Role Code cannot be empty");

            }

            var role = _daRole.EditRole(roleCode);
            if(role == null)
            {
                return Result<RoleResponseModel>.NotFoundError("Role with the given role code cannot be found");
            }
            return role;
        }

        public Result<int> UpdateRole(RoleUpdateRequestModel role, string roleCode)
        {
            if (string.IsNullOrWhiteSpace(roleCode))
            {
                return Result<int>.ValidationError("Role code cannot be empty");

            }

            if (role == null)
            {
                return Result<int>.NotFoundError("Updated role cannot be empty");
            }

            var result = _daRole.UpdateRole(role, roleCode);
            return result;
        }

        public Result<int> DeleteRole(string roleCode)
        {
            if (string.IsNullOrWhiteSpace(roleCode))
            {
                return Result<int>.ValidationError("Role code cannot be empty");
            }

            var result = _daRole.DeleteRole(roleCode);
            return result;




        }
        
    }
        
}
