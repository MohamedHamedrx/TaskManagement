using TaskManagement.Application.Contracts.Repositories;
using TaskManagement.Application.Contracts.UnitOfWork;
using TaskManagement.Infrastructure.Persistence.Context;

namespace TaskManagement.Infrastructure.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    public IProjectRepository Projects { get; }
    public ITaskRepository TaskItems { get; }
    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Projects = new ProjectRepository(_context);
        TaskItems = new TaskRepository(_context);
    }
    public async Task<int> CommitAsync() => await _context.SaveChangesAsync();

    public Task<int> CommitAsync(CancellationToken cancellationToken) => _context.SaveChangesAsync(cancellationToken);
}
