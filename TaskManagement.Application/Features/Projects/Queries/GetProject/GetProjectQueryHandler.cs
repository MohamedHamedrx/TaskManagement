using MediatR;
using TaskManagement.Application.Contracts.UnitOfWork;
using TaskManagement.Application.Features.Projects.Shared.DTO;

namespace TaskManagement.Application.Features.Projects.Queries.GetProject;

public class GetProjectQueryHandler : IRequestHandler<GetProjectQuery, ProjectDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetProjectQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ProjectDto> Handle(GetProjectQuery request, CancellationToken cancellationToken)
    {
        var project = await _unitOfWork.Projects.GetByIdAsync(request.Id);
        if (project == null)
        {
            throw new KeyNotFoundException($"Project with Id {request.Id} not found.");
        }
        var projectDto = new ProjectDto
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description!
        };
        return projectDto;
    }
}
