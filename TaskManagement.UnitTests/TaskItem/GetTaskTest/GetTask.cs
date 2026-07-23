using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Features.TaskItems.Commands.CreateTask;
using TaskManagement.Application.Features.TaskItems.Queries.GetTaskById;
using TaskManagement.Domain.Enums;
using TaskManagement.Infrastructure.Persistence.Context;
using TaskManagement.Infrastructure.Persistence.Repositories;

namespace TaskManagement.UnitTests.TaskItem.GetTaskTest;

public class GetTask
{
    [Fact]
    public async Task GetTask_WhenTaskExists_ShouldReturnTask()
    {
        // arrange 
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var context = new InMemoryDbContext(options);
        var unitOfWork = new UnitOfWork(context);
        var createHandler = new CreateTaskCommandHandler(unitOfWork);
        var getHandler = new GetTaskByIdQueryHandler(unitOfWork);
        var createCommand = new CreateTaskCommand
        {
            Title = "Test Task",
            Description = "Test Description",
            ProjectName = "Test Project",
            DueDate = DateTime.Today.AddDays(1),
            Priority = TaskPriority.Medium,
            Status = TaskItemStatus.ToDo

        };
        await createHandler.Handle(createCommand, CancellationToken.None);
        var taskItem = await context.TaskItems.FirstOrDefaultAsync(a => a.Title == "Test Task");
        var getQuery = new GetTaskByIdQuery
        {
            Id = taskItem.Id
        };
        var result = await getHandler.Handle(getQuery, CancellationToken.None);
        Assert.NotNull(result);
        Assert.Equal("Test Task", result.Title);
        Assert.Equal("Test Description", result.Description);
    }
    [Fact]
    public async Task GetTask_WhenTaskDoesNotExist_ShouldThrowKeyNotFoundException()
    {
        // arrange 
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var context = new InMemoryDbContext(options);
        var unitOfWork = new UnitOfWork(context);
        var getHandler = new GetTaskByIdQueryHandler(unitOfWork);
        var getQuery = new GetTaskByIdQuery
        {
            Id = Guid.NewGuid()
        };
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            getHandler.Handle(getQuery, CancellationToken.None));
    }
}
