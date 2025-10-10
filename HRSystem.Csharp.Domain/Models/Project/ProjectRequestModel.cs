using HRSystem.Csharp.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace HRSystem.Csharp.Domain.Models.Project;

public class ProjectRequestModel
{
        //[Required(ErrorMessage = "Project code is required.")]
        //public string ProjectCode { get; set; }

        [Required(ErrorMessage = "Project name is required.")]
        public string ProjectName { get; set; }

        [Required(ErrorMessage = "Project description is required.")]
        public string ProjectDescription { get; set; }

        [Required(ErrorMessage = "Start date is required.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required.")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Project status is required.")]
        public EnumProjectStatus ProjectStatus { get; set; }
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
