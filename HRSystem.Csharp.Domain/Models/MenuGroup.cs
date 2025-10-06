using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Csharp.Domain.Models
{
    public class MenuGroup
    {
        [Required]
        public string MenuGroupId { get; set; } = null!;
        [Required]
        public string MenuGroupCode { get; set; } = null!;
        [Required]
        public string MenuGroupName { get; set; } = null!;

        public string? Url { get; set; }

        public string? Icon { get; set; }

        public int? SortOrder { get; set; }

        public bool? HasMenuGroup { get; set; }

        public DateTime? CreatedAt { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public string? ModifiedBy { get; set; }

        public bool DeleteFlag { get; set; } = false;

    }
}
