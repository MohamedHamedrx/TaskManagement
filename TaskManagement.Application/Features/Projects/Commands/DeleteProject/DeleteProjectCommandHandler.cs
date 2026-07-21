using MediatR;
using TaskManagement.Application.Contracts.UnitOfWork;
using TaskManagement.Domain.Exceptions;

namespace TaskManagement.Application.Features.Projects.Commands.DeleteProject;

public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProjectCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _unitOfWork.Projects.GetByIdAsync(request.Id);
        if (project == null)
        {
            throw new NotFoundException($"Project with Id {request.Id} not found.");
        }

        _unitOfWork.Projects.Delete(project);
        await _unitOfWork.CommitAsync();
    }
}
