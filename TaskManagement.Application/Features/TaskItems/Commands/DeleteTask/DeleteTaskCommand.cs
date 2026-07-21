using MediatR;

namespace TaskManagement.Application.Features.TaskItems.Commands.DeleteTask;

public class DeleteTaskCommand : IRequest
{
    public Guid Id { get; set; }
}
