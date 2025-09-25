namespace HRSystem.Csharp.Domain.Models.Project;

public class ProjectResponseModel
{
        public string ProjectCode { get; set; }

        public string? ProjectName { get; set; }

        public string? ProjectDescription { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string? ProjectStatus { get; set; }

        public DateTime? CreatedAt { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public string? ModifiedBy { get; set; }
}
