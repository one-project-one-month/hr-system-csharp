using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Csharp.Domain.Models
{
    public class MenuGroupRequestModel
    {

        public string MenuGroupCode { get; set; } = null!;

        public string? MenuGroupName { get; set; }

        public string? Url { get; set; }

        public string? Icon { get; set; }

        public int? SortOrder { get; set; }

        public bool? HasMenuGroup { get; set; }

        //public string? CreatedBy { get; set; }
    }
}
