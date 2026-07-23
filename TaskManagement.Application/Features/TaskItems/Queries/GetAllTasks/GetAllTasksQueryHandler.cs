using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Common.Models.Responses;
using TaskManagement.Application.Contracts.UnitOfWork;
using TaskManagement.Application.Features.TaskItems.Shared.DTO;
using TaskManagement.Domain.Entities.Projects;

namespace TaskManagement.Application.Features.TaskItems.Queries.GetAllTasks;

public class GetAllTasksQueryHandler : IRequestHandler<GetAllTasksQuery, PaginationResult<TaskItemDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllTasksQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PaginationResult<TaskItemDto>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
    {
        var taskItems = await _unitOfWork.TaskItems.Query.AsNoTracking().Include(t => t.Project).ToListAsync();
        if (!string.IsNullOrEmpty(request.FilterBy.ToString()) && !string.IsNullOrEmpty(request.FilterType.ToString()))
        {
            taskItems = taskItems.Where(t => t.GetType().GetProperty(request.FilterBy.ToString())?.GetValue(t)?.ToString() == request.FilterType).ToList();
        }
        if (!string.IsNullOrEmpty(request.SortBy.ToString()) && !string.IsNullOrEmpty(request.SortType.ToString()))
        {
            taskItems = request.SortType.ToString().ToLower() == "asc" ? taskItems.OrderBy(t => t.GetType().GetProperty(request.SortBy.ToString())?.GetValue(t)).ToList() : taskItems.OrderByDescending(t => t.GetType().GetProperty(request.SortBy.ToString())?.GetValue(t)).ToList();
        }
        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            taskItems = taskItems.Where(t => t.Title.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) || (t.Description != null && t.Description.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase))).ToList();
        }
        var totalResults = taskItems.Count;
        var result = taskItems.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).Select(t => new TaskItemDto
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            DueDate = t.DueDate,
            ProjectName = t.Project.Name,
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
