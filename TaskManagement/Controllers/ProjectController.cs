using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Features.Projects.Commands.CreateProject;
using TaskManagement.Application.Features.Projects.Commands.DeleteProject;
using TaskManagement.Application.Features.Projects.Commands.UpdateProject;
using TaskManagement.Application.Features.Projects.Queries.GetAllProjects;
using TaskManagement.Application.Features.Projects.Queries.GetProject;
using TaskManagement.Application.Features.TaskItems.Queries.GetTaskByProjectId;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Controllers;

[ApiController]
[Route("api/projects")]
public class ProjectController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProjectController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProjectById(Guid id)
    {
        var result = await _mediator.Send(new GetProjectQuery { Id = id });
        return Ok(result);
    }
    [HttpGet]
    public async Task<IActionResult> GetAllProjects(int pageNumber= 1, int pageSize= 10)
    {
        var result = await _mediator.Send(new GetAllProjectsQuery { PageNumber = pageNumber, PageSize = pageSize });
        return Ok(result);
    }
    [HttpGet("{projectName}/tasks")]
    public async Task<IActionResult> GetTasksByProject(string projectName, int pageNumber = 1, int pageSize = 10,TaskFilterBy? filterBy = null, string filterType = null, TaskSortBy? sortBy = null, SortType? sortType = null)
    {
        var result = await _mediator.Send(new GetTaskByProjectIdQuery { ProjectName = projectName, PageNumber = pageNumber, PageSize = pageSize, FilterBy = filterBy, FilterType = filterType, SortBy = sortBy, SortType = sortType });
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProject(CreateProjectCommand command)
    {
        await _mediator.Send(command);
        return Created();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProject(Guid id, UpdateProjectCommand command)
    {
        command.Id = id;
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProject(Guid id)
    {
        await _mediator.Send(new DeleteProjectCommand { Id = id });
        return NoContent();
    }
}