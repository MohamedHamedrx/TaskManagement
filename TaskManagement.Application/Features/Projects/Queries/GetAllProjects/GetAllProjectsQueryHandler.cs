using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Common.Models.Responses;
using TaskManagement.Application.Contracts.UnitOfWork;
using TaskManagement.Application.Features.Projects.Shared.DTO;

namespace TaskManagement.Application.Features.Projects.Queries.GetAllProjects;

public class GetAllProjectsQueryHandler : IRequestHandler<GetAllProjectsQuery, PaginationResult<ProjectDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllProjectsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PaginationResult<ProjectDto>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
    {var projectsCount = await _unitOfWork.Projects.CountAsync();

    var query = _unitOfWork.Projects.Query;

    var paginatedProjects = await query
        .Skip((request.PageNumber - 1) * request.PageSize)
        .Take(request.PageSize)
        .ToListAsync(cancellationToken);

    var projectsDto = paginatedProjects.Select(p => new ProjectDto
    {
        Id = p.Id,
        Name = p.Name,
        Description = p.Description,
        

    }).ToList();

    return new PaginationResult<ProjectDto>(
        request.PageNumber,
        request.PageSize,
        projectsCount,
        projectsDto
    );
    }
}
