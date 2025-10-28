namespace HRSystem.Csharp.Domain.Models.Menu;

public class MenuRequestModel
{
    [Required(ErrorMessage = "Menu-group code is required")]
    public string MenuGroupCode { get; set; } = "";

    [Required(ErrorMessage = "Menu code is required")]
    public string MenuCode { get; set; } = "";

    [Required(ErrorMessage = "Menu name is required")]
    public string MenuName { get; set; } = "";

    public string? Url { get; set; }

    public string? Icon { get; set; }

    public int? SortOrder { get; set; }
}