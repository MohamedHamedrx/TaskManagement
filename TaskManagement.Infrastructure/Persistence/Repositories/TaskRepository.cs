using TaskManagement.Application.Contracts.Repositories;
using TaskManagement.Domain.Entities.TaskItems;
using TaskManagement.Infrastructure.Persistence.Context;

namespace TaskManagement.Infrastructure.Persistence.Repositories;

internal class TaskRepository : BaseRepository<TaskItem>, ITaskRepository
{
    private readonly AppDbContext _context;

    public TaskRepository(AppDbContext context): base(context)
    {
        _context = context;
    }

}
