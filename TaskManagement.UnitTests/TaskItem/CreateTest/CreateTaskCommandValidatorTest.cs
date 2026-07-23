using TaskManagement.Application.Features.TaskItems.Commands.CreateTask;

namespace TaskManagement.UnitTests.TaskItem.CreateTest;

public class CreateTaskCommandValidatorTest
{
    private readonly CreateTaskCommandValidator _validator;

    public CreateTaskCommandValidatorTest(CreateTaskCommandValidator validator)
    {
        _validator = validator;
    }

    [Fact]
    public void Validate_WhenTitleIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateTaskCommand
        {
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
        var command = new CreateTaskCommand
        {
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
        var command = new CreateTaskCommand
        {
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
        var command = new CreateTaskCommand
        {
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
        var command = new CreateTaskCommand
        {
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
