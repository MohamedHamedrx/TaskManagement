using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskManagement.Application.Features.TaskItems.Commands.CreateTask;
using TaskManagement.Application.Features.TaskItems.Commands.UpdateTask;
using TaskManagement.Infrastructure.Persistence.Context;
using TaskManagement.Infrastructure.Persistence.Repositories;

namespace TaskManagement.UnitTests.TaskItem.UpdateTest;

public class UpdateTask
{
    [Fact]
    public async Task UpdateTask_WhenCommandIsValid_ShouldUpdateTaskInDatabase()
    {
        // arrange 
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var logger = new LoggerFactory().CreateLogger<UpdateTaskCommandHandler>();
        var context = new InMemoryDbContext(options);
        var unitOfWork = new UnitOfWork(context);
        var createHandler = new CreateTaskCommandHandler(unitOfWork);
        var updateHandler = new UpdateTaskCommandHandler(unitOfWork, logger);
        var createCommand = new CreateTaskCommand
        {
            Title = "Test Task",
            Description = "Test Description",
            ProjectName = "Test Project",
            DueDate = DateTime.Today.AddDays(1)
        };
        await createHandler.Handle(createCommand, CancellationToken.None);
        var taskItem = await context.TaskItems.FirstOrDefaultAsync(a => a.Title == "Test Task");
        var updateCommand = new UpdateTaskCommand
        {
            Id = taskItem.Id,
            Title = "Updated Task",
            Description = "Updated Description",
            ProjectName = "Updated Project",
            DueDate = DateTime.Today.AddDays(2)
        };
        await updateHandler.Handle(updateCommand, CancellationToken.None);
        var updatedTaskItem = await context.TaskItems.FirstOrDefaultAsync(a => a.Id == taskItem.Id);
        Assert.NotNull(updatedTaskItem);
        Assert.Equal("Updated Task", updatedTaskItem.Title);
        Assert.Equal("Updated Description", updatedTaskItem.Description);
        Assert.Equal(DateTime.Today.AddDays(2), updatedTaskItem.DueDate);
    }
    [Fact]
    public async Task UpdateTask_WhenTaskDoesNotExist_ShouldThrowNotFoundException()
    {
        // arrange 
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var logger = new LoggerFactory().CreateLogger<UpdateTaskCommandHandler>();
        var context = new InMemoryDbContext(options);
        var unitOfWork = new UnitOfWork(context);
        var updateHandler = new UpdateTaskCommandHandler(unitOfWork,logger);
        var updateCommand = new UpdateTaskCommand
        {
            Id = Guid.NewGuid(),
            Title = "Updated Task",
            Description = "Updated Description",
            ProjectName = "Updated Project",
            DueDate = DateTime.Today.AddDays(2)
        };
        await Assert.ThrowsAsync<Domain.Exceptions.NotFoundException>(() =>
            updateHandler.Handle(updateCommand, CancellationToken.None));
    }
}
