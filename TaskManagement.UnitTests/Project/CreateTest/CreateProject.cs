using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Features.Projects.Commands.CreateProject;
using TaskManagement.Domain.Entities.Projects;
using TaskManagement.Infrastructure.Persistence.Context;
using TaskManagement.Infrastructure.Persistence.Repositories;

namespace TaskManagement.UnitTests.Project.CreateTest
{
    public class CreateProject
    {
        [Fact]
        public async Task CreateProject_WhenCommandIsValid_ShouldSaveProjectInDatabase()
        {
            // arrange 
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new InMemoryDbContext(options);
            var unitOfWork = new UnitOfWork(context);
            var handler = new CreateProjectCommandHandler(unitOfWork);
            var command = new CreateProjectCommand
            {
                Name = "Test Project",
                Description = "Test Description"
            };
            await handler.Handle(command, CancellationToken.None);
            var project = await context.Projects.FirstOrDefaultAsync(a => a.Name == "Test Project");

            Assert.NotNull(project);
            Assert.Equal("Test Description", project.Description);
        }
        [Fact]
        public async Task CreateProject_WhenTheNameIsDuplicate_ShouldNotSaveProjectInDatabase()
        {
            // arrange 
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new InMemoryDbContext(options);
            var unitOfWork = new UnitOfWork(context);
            var handler = new CreateProjectCommandHandler(unitOfWork);
            var command = new CreateProjectCommand
            {
                Name = "Test Project",
                Description = "Test Description"
            };
            await handler.Handle(command, CancellationToken.None);
            var project = await context.Projects.FirstOrDefaultAsync(a => a.Name == "Test Project");

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                handler.Handle(command, CancellationToken.None));

            Assert.Single(context.Projects);
        }

    }
}
