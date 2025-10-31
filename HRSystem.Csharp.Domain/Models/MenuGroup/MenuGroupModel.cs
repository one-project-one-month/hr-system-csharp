namespace HRSystem.Csharp.Domain.Models.MenuGroup;

public class MenuGroupModel
{
    public string MenuGroupId { get; set; } = null!;

    public string MenuGroupCode { get; set; } = null!;

    public string? MenuGroupName { get; set; }

    public bool? HasMenuItem { get; set; }

    public string? Url { get; set; }

    public string? Icon { get; set; }

    public int? SortOrder { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public bool DeleteFlag { get; set; } = false;
}