using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Csharp.Domain.Models.Roles
{
    public class RoleResponseModel
    {
        public string RoleId { get; set; } = null!;
        public string RoleCode { get; set; } = null!;
        public string? RoleName { get; set; }
        public string? UniqueName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public bool? DeleteFlag { get; set; }
    }
}
