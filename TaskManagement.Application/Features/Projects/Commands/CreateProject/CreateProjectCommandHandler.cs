using MediatR;
using TaskManagement.Application.Contracts.UnitOfWork;
using TaskManagement.Domain.Entities.Projects;

namespace TaskManagement.Application.Features.Projects.Commands.CreateProject;

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateProjectCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = new Project
        {
            Name = request.Name,
            Description = request.Description,
            CreatedAt = DateTime.UtcNow
            
        };
        await _unitOfWork.Projects.InsertAsync(project);
        await _unitOfWork.CommitAsync();
    }
}
