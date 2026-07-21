using MediatR;

namespace TaskManagement.Application.Features.Projects.Commands.CreateProject;

public class CreateProjectCommand : IRequest
{
    public string Name { get; set; }
    public string? Description { get; set; }

}
