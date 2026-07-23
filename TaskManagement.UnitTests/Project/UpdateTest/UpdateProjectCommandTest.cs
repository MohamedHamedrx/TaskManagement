using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Features.Projects.Commands.CreateProject;
using TaskManagement.Application.Features.Projects.Commands.UpdateProject;
using TaskManagement.Infrastructure.Persistence.Context;
using TaskManagement.Infrastructure.Persistence.Repositories;

namespace TaskManagement.UnitTests.Project.UpdateTest;

public class UpdateProjectCommandTest
{
    [Fact]
    public async Task UpdateProject_WhenCommandIsValid_ShouldUpdateProjectInDatabase()
    {
        // arrange 
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var context = new InMemoryDbContext(options);
        var unitOfWork = new UnitOfWork(context);
        var createHandler = new CreateProjectCommandHandler(unitOfWork);
        var updateHandler = new UpdateProjectCommandHandler(unitOfWork);
        var createCommand = new CreateProjectCommand
        {
            Name = "Test Project",
            Description = "Test Description"
        };
        await createHandler.Handle(createCommand, CancellationToken.None);
        var project = await context.Projects.FirstOrDefaultAsync(a => a.Name == "Test Project");
        var updateCommand = new UpdateProjectCommand
        {
            Id = project.Id,
            Name = "Updated Project",
            Description = "Updated Description"
        };
        await updateHandler.Handle(updateCommand, CancellationToken.None);
        var updatedProject = await context.Projects.FirstOrDefaultAsync(a => a.Id == project.Id);
        Assert.NotNull(updatedProject);
        Assert.Equal("Updated Project", updatedProject.Name);
        Assert.Equal("Updated Description", updatedProject.Description);
    }
    [Fact]
    public async Task UpdateProject_WhenProjectDoesNotExist_ShouldThrowNotFoundException()
    {
        // arrange 
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var context = new InMemoryDbContext(options);
        var unitOfWork = new UnitOfWork(context);
        var updateHandler = new UpdateProjectCommandHandler(unitOfWork);
        var updateCommand = new UpdateProjectCommand
        {
            Id = Guid.NewGuid(),
            Name = "Updated Project",
            Description = "Updated Description"
        };
        await Assert.ThrowsAsync<Domain.Exceptions.NotFoundException>(() =>
            updateHandler.Handle(updateCommand, CancellationToken.None));
    }
    [Fact]
    public async Task UpdateProject_WhenTheNameIsDuplicate_ShouldNotUpdateProjectInDatabase()
    {
        // arrange 
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var context = new InMemoryDbContext(options);
        var unitOfWork = new UnitOfWork(context);
        var createHandler = new CreateProjectCommandHandler(unitOfWork);
        var updateHandler = new UpdateProjectCommandHandler(unitOfWork);
        var createCommand1 = new CreateProjectCommand
        {
            Name = "Test Project 1",
            Description = "Test Description 1"
        };
        var createCommand2 = new CreateProjectCommand
        {
            Name = "Test Project 2",
            Description = "Test Description 2"
        };
        await createHandler.Handle(createCommand1, CancellationToken.None);
        await createHandler.Handle(createCommand2, CancellationToken.None);
        var project1 = await context.Projects.FirstOrDefaultAsync(a => a.Name == "Test Project 1");
        var updateCommand = new UpdateProjectCommand
        {
            Id = project1.Id,
            Name = "Test Project 2",
            Description = "Updated Description"
        };
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            updateHandler.Handle(updateCommand, CancellationToken.None));
    }
}
