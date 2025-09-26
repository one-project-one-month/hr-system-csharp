using HRSystem.Csharp.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRSystem.Csharp.Domain.Models;
using HRSystem.Csharp.Domain.Models.Roles;

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

        public Result<bool> CreateRole(RoleRequestModel role)
        {
            if (role == null)
            {
                return Result<bool>.InvalidDataError("New role cannot be empty");
            }
            var response = _daRole.CreateRole(role);
            return response;
        }

        public Result<RoleResponseModel> GetRoleByCode(string roleCode)
        {
            if (string.IsNullOrEmpty(roleCode))
            {
                return Result<RoleResponseModel>.InvalidDataError("Role code cannot be empty");
            }
            var response = _daRole.GetRoleByCode(roleCode);
            return response;
        }

        public Result<bool> UpdateRole(RoleUpdateRequestModel role, string roleCode)
        {
            if (string.IsNullOrEmpty(roleCode))
            {
                return Result<bool>.InvalidDataError("Role code cannot be empty");
            }
            else if (role == null)
            {
                return Result<bool>.InvalidDataError("Updated role cannot be empty");
            }
            var response = _daRole.UpdateRole(role, roleCode);
            return response;

        }

        public Result<bool> DeleteRole(string roleCode)
        {
            if (string.IsNullOrEmpty(roleCode))
            {
                return Result<bool>.InvalidDataError("Role code cannot be empty");

            }
            var response = _daRole.DeleteRole(roleCode);
            return response;
        }
    }

}