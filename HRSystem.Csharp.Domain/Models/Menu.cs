using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Csharp.Domain.Models
{
    public class Menu
    {
        public string MenuCode { get; set; } = null!;

        public string? MenuGroupCode { get; set; }

        public string? MenuName { get; set; }

        public string? Url { get; set; }

        public string? Icon { get; set; }

        public int? SortOrder { get; set; }

        public string? CreatedBy { get; set; }
    }
}
