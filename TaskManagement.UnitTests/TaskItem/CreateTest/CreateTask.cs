using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Features.TaskItems.Commands.CreateTask;
using TaskManagement.Domain.Enums;
using TaskManagement.Infrastructure.Persistence.Context;
using TaskManagement.Infrastructure.Persistence.Repositories;

namespace TaskManagement.UnitTests.TaskItem.CreateTest;

public class CreateTask
{
    [Fact]
    public async Task CreateTask_WhenCommandIsValid_ShouldSaveTaskInDatabase()
    {
        // arrange 
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var context = new InMemoryDbContext(options);
        var unitOfWork = new UnitOfWork(context);
        var handler = new CreateTaskCommandHandler(unitOfWork);
        var command = new CreateTaskCommand
        {
            Title = "Test Task",
            Description = "Test Description",
            DueDate = DateTime.Now.AddDays(7),
            ProjectName = "Test Project",
            Status = TaskItemStatus.Todo,
            Priority = TaskPriority.Medium
        };
        await handler.Handle(command, CancellationToken.None);
        var task = await context.TaskItems.FirstOrDefaultAsync(a => a.Title == "Test Task");
        Assert.NotNull(task);
        Assert.Equal("Test Description", task.Description);
    }
    [Fact]

}
