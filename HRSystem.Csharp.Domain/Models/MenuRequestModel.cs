using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Csharp.Domain.Models
{
    public class MenuRequestModel
    {
        [Required]
        public string MenuGroupCode { get; set; } = "";
        [Required]
        public string MenuCode { get; set; } = "";
        [Required]
        public string MenuName { get; set; } = "";
        public string? Url { get; set; }
        public string? Icon { get; set; }
        public int? SortOrder { get; set; }

    }
}
