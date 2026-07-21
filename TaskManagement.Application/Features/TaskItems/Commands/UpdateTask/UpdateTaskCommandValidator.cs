using FluentValidation;

namespace TaskManagement.Application.Features.TaskItems.Commands.UpdateTask;

public class UpdateTaskCommandValidator : AbstractValidator<UpdateTaskCommand>
{
    public UpdateTaskCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Task title is required.")
            .MaximumLength(150)
            .WithMessage("Task title cannot exceed 150 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .WithMessage("Description cannot exceed 1000 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        RuleFor(x => x.ProjectName)
            .NotEmpty()
            .WithMessage("Project name is required.");

        RuleFor(x => x.DueDate)
            .GreaterThanOrEqualTo(DateTime.Today)
            .WithMessage("Due date cannot be in the past.");
    }
}
