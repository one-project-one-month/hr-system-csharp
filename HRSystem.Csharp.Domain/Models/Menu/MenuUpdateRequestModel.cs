using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Csharp.Domain.Models.Menu
{
    public class MenuUpdateRequestModel
    {

        [Required(ErrorMessage = "Menu-group code is required")]
        public string MenuGroupCode { get; set; } = null!;

        [Required(ErrorMessage = "Menu name is required")]
        public string MenuName { get; set; } = null!;

        public string? Url { get; set; }

        public string? Icon { get; set; }

        public int? SortOrder { get; set; }
    }
}
