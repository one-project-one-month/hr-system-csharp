using HRSystem.Csharp.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRSystem.Csharp.Domain.Models;

namespace HRSystem.Csharp.Domain.Features
{
    public class BL_Role
    {
        private readonly DA_Role _daRole;

        public BL_Role(DA_Role daRole)
        {
            _daRole = daRole;
        }

        public Result<List<Role>> GetAllRoles()
        {
            var roles = _daRole.GetAllRoles();
            var response = Result<List<Role>>.Success(roles.Data);
            return response;
        }
    }
}
