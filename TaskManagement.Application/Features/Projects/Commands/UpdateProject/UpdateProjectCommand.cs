using MediatR;
using System.Text.Json.Serialization;

namespace TaskManagement.Application.Features.Projects.Commands.UpdateProject;

public class UpdateProjectCommand : IRequest
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
}
