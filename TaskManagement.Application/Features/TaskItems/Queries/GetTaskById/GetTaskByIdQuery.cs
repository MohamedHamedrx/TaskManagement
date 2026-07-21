using MediatR;
using TaskManagement.Application.Features.TaskItems.Shared.DTO;

namespace TaskManagement.Application.Features.TaskItems.Queries.GetTaskById;

public class GetTaskByIdQuery : IRequest<TaskItemDto>
{
    public Guid Id { get; set; }
}
