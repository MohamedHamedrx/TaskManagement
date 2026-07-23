using MediatR;
using TaskManagement.Application.Common.Models.Responses;
using TaskManagement.Application.Features.Projects.Shared.DTO;

namespace TaskManagement.Application.Features.Projects.Queries.GetAllProjects;

public class GetAllProjectsQuery : IRequest<PaginationResult<ProjectDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;

}
