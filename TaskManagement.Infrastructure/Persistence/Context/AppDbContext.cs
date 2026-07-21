using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities.Projects;
using TaskManagement.Domain.Entities.TaskItems;

namespace TaskManagement.Infrastructure.Persistence.Context;

public class AppDbContext : DbContext
{
    public DbSet<Project> Projects { get; set; }
    public DbSet<TaskItem> TaskItems { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options) :
    base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
