using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Features.TaskItems.Commands.CreateTask;
using TaskManagement.Application.Features.TaskItems.Queries.GetTaskByProjectId;
using TaskManagement.Infrastructure.Persistence.Context;
using TaskManagement.Infrastructure.Persistence.Repositories;

namespace TaskManagement.UnitTests.TaskItem.GetAllProjectTasksTest;

public class GetAllProjectTasks
{
    [Fact]
    public async Task GetAllProjectTasks_WhenProjectExists_ShouldReturnAllTasksForProject()
    {
        // arrange 
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var context = new InMemoryDbContext(options);
        var unitOfWork = new UnitOfWork(context);
        var createHandler = new CreateTaskCommandHandler(unitOfWork);
        var getAllHandler = new GetTaskByProjectIdQueryHandler(unitOfWork);
        var createCommand1 = new CreateTaskCommand
        {
            Title = "Test Task 1",
            Description = "Test Description 1",
            ProjectName = "Test Project",
            DueDate = DateTime.Today.AddDays(1)
        };
        var createCommand2 = new CreateTaskCommand
        {
            Title = "Test Task 2",
            Description = "Test Description 2",
            ProjectName = "Test Project",
            DueDate = DateTime.Today.AddDays(2)
        };
        await createHandler.Handle(createCommand1, CancellationToken.None);
        await createHandler.Handle(createCommand2, CancellationToken.None);
        var query = new GetTaskByProjectIdQuery
        {
            ProjectName = "Test Project"
        };
        var result = await getAllHandler.Handle(query, CancellationToken.None);
        Assert.NotNull(result);
        Assert.Equal(2, result.Items.Count);
    }
    [Fact]
    public async Task GetAllProjectTasks_WhenProjectDoesNotExist_ShouldReturnEmptyList()
    {
        // arrange 
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var context = new InMemoryDbContext(options);
        var unitOfWork = new UnitOfWork(context);
        var getAllHandler = new GetTaskByProjectIdQueryHandler(unitOfWork);
        var query = new GetTaskByProjectIdQuery
        {
            ProjectName = "NonExistentProject"
        };
        var result = await getAllHandler.Handle(query, CancellationToken.None);
        Assert.NotNull(result);
        Assert.Empty(result.Items);
    }
}
