using MediatR;
using TaskManagement.Application.Contracts.UnitOfWork;

namespace TaskManagement.Application.Features.TaskItems.Commands.UpdateTask;

public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTaskCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var taskItem = await _unitOfWork.TaskItems.GetByIdAsync(request.Id);
        if (taskItem == null)
        {
            throw new Exception($"Task with Id {request.Id} not found.");
        }
        var project = await _unitOfWork.Projects.GetAsync(p => p.Name == request.ProjectName);
        if (project == null)
        {
            throw new Exception($"Project with name {request.ProjectName} not found.");
        }
        taskItem.Title = request.Title;
        taskItem.Description = request.Description;
        taskItem.DueDate = request.DueDate;
        taskItem.Status = request.Status;
        taskItem.Priority = request.Priority;
        taskItem.ProjectId = project.Id;
        taskItem.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.CommitAsync(cancellationToken);
    }
}
