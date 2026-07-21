using TaskManagement.Application.Contracts.Repositories;

namespace TaskManagement.Application.Contracts.UnitOfWork;

public interface IUnitOfWork
{
    IProjectRepository Projects { get; }
    ITaskRepository TaskItems { get; }
    Task<int> CommitAsync();
    Task<int> CommitAsync(CancellationToken cancellationToken);
}
