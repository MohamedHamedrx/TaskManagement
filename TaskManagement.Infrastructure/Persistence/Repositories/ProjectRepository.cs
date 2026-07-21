using TaskManagement.Application.Contracts.Repositories;
using TaskManagement.Domain.Entities.Projects;
using TaskManagement.Infrastructure.Persistence.Context;

namespace TaskManagement.Infrastructure.Persistence.Repositories;

internal class ProjectRepository : BaseRepository<Project>, IProjectRepository
{
    private readonly AppDbContext _context;

    public ProjectRepository(AppDbContext context): base(context)
    {
        _context = context;
    }
}
