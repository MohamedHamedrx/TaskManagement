using MediatR;
using TaskManagement.Application.Contracts.UnitOfWork;
using TaskManagement.Domain.Exceptions;

namespace TaskManagement.Application.Features.Projects.Commands.UpdateProject;

public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProjectCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _unitOfWork.Projects.GetByIdAsync(request.Id);
        if (project == null)
        {
            throw new NotFoundException($"Project with Id {request.Id} not found.");
        }

        project.Name = request.Name;
        project.Description = request.Description;
        project.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.CommitAsync(cancellationToken);
    }
}
