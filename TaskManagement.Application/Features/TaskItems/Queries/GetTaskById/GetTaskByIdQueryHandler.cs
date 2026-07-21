using MediatR;
using TaskManagement.Application.Contracts.UnitOfWork;
using TaskManagement.Application.Features.TaskItems.Shared.DTO;

namespace TaskManagement.Application.Features.TaskItems.Queries.GetTaskById;

public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, TaskItemDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTaskByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TaskItemDto> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        var taskItem = await _unitOfWork.TaskItems.GetByIdAsync(request.Id);
        if (taskItem == null)
        {
            throw new Exception($"Task with Id {request.Id} not found.");
        }
        var project = await _unitOfWork.Projects.GetByIdAsync(taskItem.ProjectId);
        if (project == null)
            {
            throw new Exception($"Project with Id {taskItem.ProjectId} not found.");
        }
        var taskItemDto = new TaskItemDto
        {
            Id = taskItem.Id,
            Title = taskItem.Title,
            Description = taskItem.Description,
            DueDate = taskItem.DueDate,
            ProjectName = project.Name,
            Status = taskItem.Status,
            Priority = taskItem.Priority
        };

        return taskItemDto;
    }
}