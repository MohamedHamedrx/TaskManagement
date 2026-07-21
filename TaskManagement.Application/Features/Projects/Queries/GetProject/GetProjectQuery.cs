using MediatR;
using TaskManagement.Application.Features.Projects.Shared.DTO;

namespace TaskManagement.Application.Features.Projects.Queries.GetProject;

public class GetProjectQuery : IRequest<ProjectDto>
{
    public Guid Id { get; set; }
}
