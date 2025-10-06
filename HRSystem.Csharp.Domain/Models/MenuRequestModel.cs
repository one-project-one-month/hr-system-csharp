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
        public required string MenuGroupCode { get; set; } = "";
        [Required]
        public required string MenuCode { get; set; } = "";
        [Required]
        public required string MenuName { get; set; } = "";
        public string? Url { get; set; }
        public string? Icon { get; set; }
        public int? SortOrder { get; set; }

    }
}
