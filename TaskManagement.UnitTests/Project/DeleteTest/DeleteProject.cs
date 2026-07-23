using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Features.Projects.Commands.CreateProject;
using TaskManagement.Application.Features.Projects.Commands.DeleteProject;
using TaskManagement.Infrastructure.Persistence.Context;
using TaskManagement.Infrastructure.Persistence.Repositories;

namespace TaskManagement.UnitTests.Project.DeleteTest;

public class DeleteProject
{
    [Fact]
    public async Task DeleteProject_WhenDataIsValid_ShouldDeleteProjectInDatabase()
    {
        // arrange 
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var context = new InMemoryDbContext(options);
        var unitOfWork = new UnitOfWork(context);
        var createHandler = new CreateProjectCommandHandler(unitOfWork);
        var deleteHandler = new DeleteProjectCommandHandler(unitOfWork);
        var createCommand = new CreateProjectCommand
        {
            Name = "Test Project",
            Description = "Test Description"
        };
        await createHandler.Handle(createCommand, CancellationToken.None);
        var project = await context.Projects.FirstOrDefaultAsync(a => a.Name == "Test Project");
        var deleteCommand = new DeleteProjectCommand
        {
            Id = project.Id
        };
        await deleteHandler.Handle(deleteCommand, CancellationToken.None);
        var deletedProject = await context.Projects.FirstOrDefaultAsync(a => a.Id == project.Id);
        Assert.Null(deletedProject);
    }
    [Fact]
    public async Task DeleteProject_WhenProjectDoesNotExist_ShouldThrowNotFoundException()
    {
        // arrange 
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var context = new InMemoryDbContext(options);
        var unitOfWork = new UnitOfWork(context);
        var deleteHandler = new DeleteProjectCommandHandler(unitOfWork);
        var deleteCommand = new DeleteProjectCommand
        {
            Id = Guid.NewGuid()
        };
        await Assert.ThrowsAsync<Domain.Exceptions.NotFoundException>(() =>
            deleteHandler.Handle(deleteCommand, CancellationToken.None));
    }

}
