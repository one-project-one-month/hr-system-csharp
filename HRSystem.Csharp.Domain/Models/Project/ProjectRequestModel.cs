using HRSystem.Csharp.Shared.Enums;

namespace HRSystem.Csharp.Domain.Models.Project;

public class ProjectCreateRequestModel
{
        public string ProjectCode { get; set; }

        public string? ProjectName { get; set; }

        public string? ProjectDescription { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
}

public class ProjectUpdateRequestModel
{
        public string? ProjectName { get; set; }

        public string? ProjectDescription { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public EnumProjectStatus ProjectStatus { get; set; }
}
