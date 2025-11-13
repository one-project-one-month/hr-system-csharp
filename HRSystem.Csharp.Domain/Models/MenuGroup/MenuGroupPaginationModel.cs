using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Csharp.Domain.Models.MenuGroup
{
    public class MenuGroupPaginationModel
    {
        public int PageNo { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? MenuGroupName { get; set; }
    }
}
