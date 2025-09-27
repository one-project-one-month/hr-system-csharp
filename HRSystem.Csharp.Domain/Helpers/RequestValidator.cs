using FluentValidation;
using FluentValidation.Results;
using HRSystem.Csharp.Domain.Models.Project;
using HRSystem.Csharp.Shared;

namespace HRSystem.Csharp.Domain.Helpers;

public class ProjectCreateValidator: AbstractValidator<ProjectCreateRequestModel>
{
        public ProjectCreateValidator()
        {
                RuleFor(p => p.ProjectCode)
                        .NotEmpty().WithMessage("Project code is required");

                RuleFor(p => p.ProjectName)
                        .NotEmpty().WithMessage("Project Name is required.");

                RuleFor(p => p.StartDate)
                        .NotEmpty().WithMessage("Project start date is required.");
                // .Must(date => date >= DateTime.Now).WithMessage("Project must not be started in the past.");

                RuleFor(p => p.EndDate)
                        .NotEmpty().WithMessage("Project end date is required.");
                       // .Must(date => date >= DateTime.Now).WithMessage("Project must not be ended in the past.");
        }
}

public class ProjectUpdateValidator : AbstractValidator<ProjectUpdateRequestModel>
{
        public ProjectUpdateValidator()
        {
                RuleFor(p => p.ProjectName)
                        .NotEmpty().WithMessage("Project Name is required.");

                RuleFor(p => p.StartDate)
                        .NotEmpty().WithMessage("Project start date is required.");
                // .Must(date => date >= DateTime.Now).WithMessage("Project must not be started in the past.");

                RuleFor(p => p.EndDate)
                        .NotEmpty().WithMessage("Project end date is required.");
                       // .Must(date => date >= DateTime.Now).WithMessage("Project must not be ended in the past.");

                RuleFor(p => p.ProjectStatus)
                        .NotNull().WithMessage("Project status is required.")
                        .IsInEnum().WithMessage("Project status must be one of: Pending, InProgress, Finished");
        }
}