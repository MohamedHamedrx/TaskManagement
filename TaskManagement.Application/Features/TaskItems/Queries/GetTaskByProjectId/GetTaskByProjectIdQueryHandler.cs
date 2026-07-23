using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TaskManagement.Application.Common.Models.Responses;
using TaskManagement.Application.Contracts.UnitOfWork;
using TaskManagement.Application.Features.Projects.Shared.DTO;
using TaskManagement.Application.Features.TaskItems.Shared.DTO;

namespace TaskManagement.Application.Features.TaskItems.Queries.GetTaskByProjectId;

public class GetTaskByProjectIdQueryHandler : IRequestHandler<GetTaskByProjectIdQuery, PaginationResult<TaskItemDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTaskByProjectIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PaginationResult<TaskItemDto>> Handle(GetTaskByProjectIdQuery request, CancellationToken cancellationToken)
    {
        var project = await _unitOfWork.Projects.GetAsync(p => p.Name == request.ProjectName);
        if (project == null)
        {
            throw new Exception($"Project with name {request.ProjectName} not found.");

        }
        var taskItems = await _unitOfWork.TaskItems.Query.AsNoTracking().Where(p => p.ProjectId == project.Id ).ToListAsync();
        if (!string.IsNullOrEmpty(request.FilterBy.ToString()) && !string.IsNullOrEmpty(request.FilterType))
        {
            taskItems = taskItems.Where(t => t.GetType().GetProperty(request.FilterBy.ToString(),
                                                          BindingFlags.Public |
                                                          BindingFlags.Instance |
                                                          BindingFlags.IgnoreCase)?.GetValue(t)?.ToString() == request.FilterType).ToList();
        }
        if (!string.IsNullOrEmpty(request.SortBy.ToString()) && !string.IsNullOrEmpty(request.SortType.ToString()))
        {
            taskItems = request.SortType.ToString().ToLower() == "asc" ? taskItems.OrderBy(t => t.GetType().GetProperty(request.SortBy.ToString(),
                                                          BindingFlags.Public |
                                                          BindingFlags.Instance |
                                                          BindingFlags.IgnoreCase)?.GetValue(t)).ToList() : taskItems.OrderByDescending(t => t.GetType().GetProperty(request.SortBy.ToString())?.GetValue(t)).ToList();
        }
        var totalResults = taskItems.Count;
        var result = taskItems.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).Select(t => new TaskItemDto
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            DueDate = t.DueDate,
            ProjectName = project.Name,
            Status = t.Status,
            Priority = t.Priority
        }).ToList();
        return new PaginationResult<TaskItemDto>(
            request.PageNumber,
            request.PageSize,
            totalResults,
            result
        );
    }
}
