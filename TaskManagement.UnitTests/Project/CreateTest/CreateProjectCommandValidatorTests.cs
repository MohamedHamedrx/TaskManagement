using TaskManagement.Application.Features.Projects.Commands.CreateProject;

namespace TaskManagement.UnitTests.Project.CreateTest;

public class CreateProjectCommandValidatorTests
{
    private readonly CreateProjectCommandValidator _validator;

    public CreateProjectCommandValidatorTests(CreateProjectCommandValidator validator)
    {
        _validator = validator;
    }

    [Fact]
    public void Validate_WhenNameIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateProjectCommand
        {
            Name = "",
            Description = "Test Description"
        };
        // Act
        var result = _validator.Validate(command);
        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Name" && e.ErrorMessage == "Project name is required.");
    }
    [Fact]
    public void Validate_WhenNameExceedsMaxLength_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateProjectCommand
        {
            Name = new string('A', 101),
            Description = "Test Description"
        };
        // Act
        var result = _validator.Validate(command);
        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Name" && e.ErrorMessage == "Project name cannot exceed 100 characters.");
    }
    [Fact]
    public void Validate_WhenDescriptionExceedsMaxLength_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateProjectCommand
        {
            Name = "Test Project",
            Description = new string('A', 501)
        };
        // Act
        var result = _validator.Validate(command);
        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Description" && e.ErrorMessage == "Description cannot exceed 500 characters.");
    }
}
