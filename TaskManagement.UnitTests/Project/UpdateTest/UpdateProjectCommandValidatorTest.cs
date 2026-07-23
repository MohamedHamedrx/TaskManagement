using TaskManagement.Application.Features.Projects.Commands.UpdateProject;

namespace TaskManagement.UnitTests.Project.UpdateTest;

public class UpdateProjectCommandValidatorTest
{
    private readonly UpdateProjectCommandValidator _validator;

    public UpdateProjectCommandValidatorTest(UpdateProjectCommandValidator validator)
    {
        _validator = validator;
    }
    [Fact]
    public void Validate_WhenNameIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        var command = new UpdateProjectCommand
        {
            Id = Guid.NewGuid(),
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
        var command = new UpdateProjectCommand
        {
            Id = Guid.NewGuid(),
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
        var command = new UpdateProjectCommand
        {
            Id = Guid.NewGuid(),
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
