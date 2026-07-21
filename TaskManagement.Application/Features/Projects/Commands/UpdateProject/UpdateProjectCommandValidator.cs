using FluentValidation;

namespace TaskManagement.Application.Features.Projects.Commands.UpdateProject;

public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
{
    public UpdateProjectCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Project name is required.")
            .MaximumLength(100)
            .WithMessage("Project name cannot exceed 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Description cannot exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
    }
}
