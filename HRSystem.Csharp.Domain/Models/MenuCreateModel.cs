using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Csharp.Domain.Models
{
    public class MenuCreateModel
    {
        public required string MenuGroupCode { get; set; }
        public required string MenuCode { get; set; }
        public required string MenuName { get; set; }
        public required string Url { get; set; }
        public required string Icon { get; set; }
        public int SortOrder { get; set; }

    }
}
