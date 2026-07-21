using MediatR;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.Features.TaskItems.Commands.CreateTask;

public class CreateTaskCommand : IRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public string ProjectName { get; set; }
    public TaskItemStatus Status { get; set; }
    public TaskPriority Priority { get; set; }

}
