using TaskManagement.Application.Features.TaskItems.Commands.UpdateTask;

namespace TaskManagement.UnitTests.TaskItem.UpdateTest;

public class UpdateTaskCommandValidatorTest
{
    private readonly UpdateTaskCommandValidator _validator;

    public UpdateTaskCommandValidatorTest(UpdateTaskCommandValidator validator)
    {
        _validator = validator;
    }
    [Fact]
    public void Validate_WhenTitleIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateTaskCommand
        {
            Id = Guid.NewGuid(),
            Title = "",
            Description = "Test Description",
            ProjectName = "Test Project",
            DueDate = DateTime.Today.AddDays(1)
        };
        // Act
        var result = _validator.Validate(command);
        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Title" && e.ErrorMessage == "Task title is required.");
    }
    [Fact]
    public void Validate_WhenTitleExceedsMaxLength_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateTaskCommand
        {
            Id = Guid.NewGuid(),
            Title = new string('A', 151),
            Description = "Test Description",
            ProjectName = "Test Project",
            DueDate = DateTime.Today.AddDays(1)
        };
        // Act
        var result = _validator.Validate(command);
        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Title" && e.ErrorMessage == "Task title cannot exceed 150 characters.");
    }
    [Fact]
    public void Validate_WhenDescriptionExceedsMaxLength_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateTaskCommand
        {
            Id = Guid.NewGuid(),
            Title = "Test Task",
            Description = new string('A', 1001),
            ProjectName = "Test Project",
            DueDate = DateTime.Today.AddDays(1)
        };
        // Act
        var result = _validator.Validate(command);
        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Description" && e.ErrorMessage == "Description cannot exceed 1000 characters.");
    }
    [Fact]
    public void Validate_WhenProjectNameIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateTaskCommand
        {
            Id = Guid.NewGuid(),
            Title = "Test Task",
            Description = "Test Description",
            ProjectName = "",
            DueDate = DateTime.Today.AddDays(1)
        };
        // Act
        var result = _validator.Validate(command);
        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "ProjectName" && e.ErrorMessage == "Project name is required.");
    }
    [Fact]
    public void Validate_WhenDueDateIsInThePast_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateTaskCommand
        {
            Id = Guid.NewGuid(),
            Title = "Test Task",
            Description = "Test Description",
            ProjectName = "Test Project",
            DueDate = DateTime.Today.AddDays(-1)
        };
        // Act
        var result = _validator.Validate(command);
        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "DueDate" && e.ErrorMessage == "Due date cannot be in the past.");
    }

}
