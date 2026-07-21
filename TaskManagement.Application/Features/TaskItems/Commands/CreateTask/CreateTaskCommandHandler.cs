using MediatR;
using TaskManagement.Application.Contracts.UnitOfWork;
using TaskManagement.Domain.Entities.TaskItems;


namespace TaskManagement.Application.Features.TaskItems.Commands.CreateTask;

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateTaskCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var project = await _unitOfWork.Projects.GetAsync(p => p.Name == request.ProjectName);
        if (project == null)
        {
            throw new Exception($"Project with name {request.ProjectName} not found.");
        }
        var task = new TaskItem
        {
            Title = request.Title,
            Description = request.Description,
            DueDate = request.DueDate,
            ProjectId = project.Id,
            Status = request.Status,
            Priority = request.Priority,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null
        };
        await _unitOfWork.TaskItems.InsertAsync(task);
        await _unitOfWork.CommitAsync();
    }
}
