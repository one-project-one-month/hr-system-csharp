using HRSystem.Csharp.Shared.Enums;

namespace HRSystem.Csharp.Domain.Models.Project;

public class ProjectRequestModel
{
    public string ProjectName { get; set; }

    public string ProjectDescription { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public EnumProjectStatus ProjectStatus { get; set; }
}

public class ProjectEditRequestModel
{
    public string ProjectCode { get; set; }
}

//public class ProjectUpdateRequestModel
//{
//        [Required(ErrorMessage = "Project code is required.")]
//        public string ProjectName { get; set; }

//        [Required(ErrorMessage = "Project description is required.")]
//        public string ProjectDescription { get; set; }

//        [Required(ErrorMessage = "Start date is required.")]
//        public DateTime StartDate { get; set; }

//        [Required(ErrorMessage = "End date is required.")]
//        public DateTime EndDate { get; set; }

//        [Required(ErrorMessage = "Project status is required.")]
//        public EnumProjectStatus ProjectStatus { get; set; }
//}