using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Features.Projects.Commands.CreateProject;
using TaskManagement.Application.Features.Projects.Queries.GetProject;
using TaskManagement.Infrastructure.Persistence.Context;
using TaskManagement.Infrastructure.Persistence.Repositories;

namespace TaskManagement.UnitTests.Project.GetProjectTest;

public class GetProject
{
    [Fact]
    public async Task GetProject_WhenProjectExists_ShouldReturnProjectDto()
    {
        // arrange 
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var context = new InMemoryDbContext(options);
        var unitOfWork = new UnitOfWork(context);
        var createHandler = new CreateProjectCommandHandler(unitOfWork);
        var getHandler = new GetProjectQueryHandler(unitOfWork);
        var createCommand = new CreateProjectCommand
        {
            Name = "Test Project",
            Description = "Test Description"
        };
        await createHandler.Handle(createCommand, CancellationToken.None);
        var project = await context.Projects.FirstOrDefaultAsync(a => a.Name == "Test Project");
        var getQuery = new GetProjectQuery
        {
            Id = project.Id
        };
        var projectDto = await getHandler.Handle(getQuery, CancellationToken.None);
        Assert.NotNull(projectDto);
        Assert.Equal("Test Project", projectDto.Name);
        Assert.Equal("Test Description", projectDto.Description);
    }
    [Fact]
    public async Task GetProject_WhenProjectDoesNotExist_ShouldThrowKeyNotFoundException()
    {
        // arrange 
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var context = new InMemoryDbContext(options);
        var unitOfWork = new UnitOfWork(context);
        var getHandler = new GetProjectQueryHandler(unitOfWork);
        var getQuery = new GetProjectQuery
        {
            Id = Guid.NewGuid()
        };
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            getHandler.Handle(getQuery, CancellationToken.None));
    }
}
