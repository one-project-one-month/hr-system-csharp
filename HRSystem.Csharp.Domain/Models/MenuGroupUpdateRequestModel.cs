using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Csharp.Domain.Models
{
    public class MenuGroupUpdateRequestModel
    {
        [Required(ErrorMessage ="Menu Group Name is required!")]
        public string MenuGroupName { get; set; } = "";

        public string? Url { get; set; }

        public string? Icon { get; set; }

        public int? SortOrder { get; set; }

        public bool? HasMenuItem { get; set; }
    }
}

