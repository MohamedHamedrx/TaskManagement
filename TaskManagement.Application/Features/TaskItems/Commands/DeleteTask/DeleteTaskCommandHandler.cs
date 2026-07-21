using MediatR;
using TaskManagement.Application.Contracts.UnitOfWork;

namespace TaskManagement.Application.Features.TaskItems.Commands.DeleteTask;

public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTaskCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        var taskItem = await _unitOfWork.TaskItems.GetByIdAsync(request.Id);
        if (taskItem == null)
        {
            throw new Exception($"Task with Id {request.Id} not found.");
        }

        _unitOfWork.TaskItems.Delete(taskItem);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}