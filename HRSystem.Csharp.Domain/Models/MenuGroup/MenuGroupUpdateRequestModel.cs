namespace HRSystem.Csharp.Domain.Models.MenuGroup;

public class MenuGroupUpdateRequestModel
{
    [Required(ErrorMessage = "Menu Group Name is required!")]
    public string MenuGroupName { get; set; } = "";

    public string? Url { get; set; }

    public string? Icon { get; set; }

    public int? SortOrder { get; set; }

    public bool? HasMenuItem { get; set; }
}