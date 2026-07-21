using TaskManagement.Domain.Entities.TaskItems;

namespace TaskManagement.Application.Contracts.Repositories;

public interface ITaskRepository : IBaseRepository<TaskItem>
{
    Task InsertAsync(TaskItem task);
}
