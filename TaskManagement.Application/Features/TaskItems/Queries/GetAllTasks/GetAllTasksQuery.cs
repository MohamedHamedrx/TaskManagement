using MediatR;
using TaskManagement.Application.Common.Models.Responses;
using TaskManagement.Application.Features.TaskItems.Shared.DTO;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.Features.TaskItems.Queries.GetAllTasks;

public class GetAllTasksQuery : IRequest<PaginationResult<TaskItemDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? FilterType { get; set; }
    public string? SearchTerm { get; set; }
    public TaskFilterBy? FilterBy { get; set; }
    public TaskSortBy? SortBy { get; set; }
    public SortType? SortType { get; set; }

}
