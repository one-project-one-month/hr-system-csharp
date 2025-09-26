using HRSystem.Csharp.Shared.Enums;
using System.Text.Json.Serialization;

namespace HRSystem.Csharp.Domain.Models.Project;

public class ProjectResponseModel
{
        public string ProjectCode { get; set; }

        public string? ProjectName { get; set; }

        public string? ProjectDescription { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EnumProjectStatus ProjectStatus { get; set; }

        public DateTime? CreatedAt { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public string? ModifiedBy { get; set; }
}
