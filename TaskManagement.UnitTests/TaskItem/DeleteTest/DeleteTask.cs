using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskManagement.Application.Features.TaskItems.Commands.CreateTask;
using TaskManagement.Application.Features.TaskItems.Commands.DeleteTask;
using TaskManagement.Infrastructure.Persistence.Context;
using TaskManagement.Infrastructure.Persistence.Repositories;

namespace TaskManagement.UnitTests.TaskItem.DeleteTest;

public class DeleteTask 
{
    [Fact]
    public async Task DeleteTask_WhenCommandIsValid_ShouldDeleteTaskFromDatabase()
    {
        // arrange 
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var context = new InMemoryDbContext(options);
        var unitOfWork = new UnitOfWork(context);
        var createHandler = new CreateTaskCommandHandler(unitOfWork);
        var deleteHandler = new DeleteTaskCommandHandler(unitOfWork);
        var createCommand = new CreateTaskCommand
        {
            Title = "Test Task",
            Description = "Test Description",
            ProjectName = "Test Project",
            DueDate = DateTime.Today.AddDays(1)
        };
        await createHandler.Handle(createCommand, CancellationToken.None);
        var taskItem = await context.TaskItems.FirstOrDefaultAsync(a => a.Title == "Test Task");
        var deleteCommand = new DeleteTaskCommand
        {
            Id = taskItem.Id
        };
        await deleteHandler.Handle(deleteCommand, CancellationToken.None);
        var deletedTaskItem = await context.TaskItems.FirstOrDefaultAsync(a => a.Id == taskItem.Id);
        Assert.Null(deletedTaskItem);
    }
    [Fact]
    public async Task DeleteTask_WhenTaskDoesNotExist_ShouldThrowNotFoundException()
    {
        // arrange 
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var context = new InMemoryDbContext(options);
        var unitOfWork = new UnitOfWork(context);
        var deleteHandler = new DeleteTaskCommandHandler(unitOfWork);
        var deleteCommand = new DeleteTaskCommand
        {
            Id = Guid.NewGuid()
        };
        await Assert.ThrowsAsync<Domain.Exceptions.NotFoundException>(() =>
            deleteHandler.Handle(deleteCommand, CancellationToken.None));
    }
}
