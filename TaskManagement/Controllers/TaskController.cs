using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Features.TaskItems.Commands.CreateTask;
using TaskManagement.Application.Features.TaskItems.Commands.DeleteTask;
using TaskManagement.Application.Features.TaskItems.Commands.UpdateTask;
using TaskManagement.Application.Features.TaskItems.Queries.GetTaskById;

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
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetTaskByIdQuery { Id = id });
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTaskCommand command)
    {
        await _mediator.Send(command);
        return Created();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateTaskCommand command)
    {
        command.Id = id;
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteTaskCommand { Id = id });
        return NoContent();
    }
}