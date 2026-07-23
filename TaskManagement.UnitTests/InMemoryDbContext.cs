using Microsoft.EntityFrameworkCore;
using TaskManagement.Infrastructure.Persistence.Context;

namespace TaskManagement.UnitTests;

public class InMemoryDbContext : AppDbContext
{
    public InMemoryDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    override protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
    }
    public override void Dispose()
    {
        Database.EnsureDeleted();
        base.Dispose();
    }
}
