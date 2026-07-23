using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Features.TaskItems.Commands.CreateTask;
using TaskManagement.Application.Features.TaskItems.Commands.DeleteTask;
using TaskManagement.Application.Features.TaskItems.Commands.UpdateTask;
using TaskManagement.Application.Features.TaskItems.Queries.GetAllTasks;
using TaskManagement.Application.Features.TaskItems.Queries.GetTaskById;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Controllers;

[ApiController]
[Route("api/tasks")]
public class TaskController : ControllerBase
{
    private readonly IMediator _mediator;

    public TaskController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTaskById(Guid id)
    {
        var result = await _mediator.Send(new GetTaskByIdQuery { Id = id });
        return Ok(result);
    }
    [HttpGet]
    public async Task<IActionResult> GetAllTasks(int pageNumber = 1, int pageSize = 10, TaskFilterBy? filterBy = null, string filterType = null, TaskSortBy? sortBy = null, SortType? sortType = null,string searchTerm = null)
    {
        var result = await _mediator.Send(new GetAllTasksQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            FilterBy = filterBy,
            FilterType = filterType,
            SortBy = sortBy,
            SortType = sortType,
            SearchTerm = searchTerm
        });
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask(CreateTaskCommand command)
    {
        await _mediator.Send(command);
        return Created();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(Guid id, UpdateTaskCommand command)
    {
        command.Id = id;
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(Guid id)
    {
        await _mediator.Send(new DeleteTaskCommand { Id = id });
        return NoContent();
    }
}