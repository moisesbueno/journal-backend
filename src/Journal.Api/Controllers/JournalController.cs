using Journal.Application.DTOs;
using Journal.Application.Journal.Queries;
using Journal.Application.Journal.Commands;
using Journal.Infrastructure.MessageBus;
using Journal.Infrastructure.MessageBus.Queues;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace Journal.Api.Controllers;

[ApiController]
[Route("api/journal")]
public class JournalController : Controller
{
    private readonly IPublisher<JournalMessage> _journalPublisher;
    private readonly IMediator _mediator;

    public JournalController(
        IPublisher<JournalMessage> journalPublisher,
        IMediator mediator)
    {
        _journalPublisher = journalPublisher;
        _mediator = mediator;
    }

    [HttpPost("")]
    public async Task<IActionResult> Add([FromBody] JournalAddRequest journalRequest)
    {
        var command = new AddJournalCommand
        {
            Title = journalRequest.Name,
            //Content = journalRequest.
            //Content = journalRequest.Content,
        };

        var result = await _mediator.Send(command);

        // if (result.IsSuccess)
        // {
        //     var journalMessage = new JournalMessage
        //     {
        //         Id = result.Value.Id,
        //         Title = result.Value.Title,
        //         Content = result.Value.Content,
        //         CreatedAt = result.Value.CreatedAt,
        //         UserId = result.Value.UserId
        //     };

        //     await _journalPublisher.SendMessageAsync(journalMessage, QueuesName.JournalQueue);

        //     return Ok(journalMessage.Id);
        // }

        return BadRequest(result.Errors);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] JournalUpdateRequest journalRequest)
    {
        var command = new UpdateJournalCommand
        {
            Id = id,
            Name = journalRequest.Title,
            Aimscope = journalRequest.Content
        };

        var result = await _mediator.Send(command);

        // if (result.IsSuccess)
        // {
        //     var journalMessage = new JournalMessage
        //     {
        //         Id = result.Value.Id,
        //         Title = result.Value.Title,
        //         Content = result.Value.Content,
        //         CreatedAt = result.Value.CreatedAt,
        //         UserId = result.Value.UserId
        //     };

        //     await _journalPublisher.SendMessageAsync(journalMessage, QueuesName.JournalQueue);

        //     return Ok(journalMessage.Id);
        // }

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetByIdQuery
        {
            Id = id
        };

        var result = await _mediator.Send(query);

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteJournalCommand
        {
            Id = id
        };

        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            return Ok();
        }

        return BadRequest(result.Errors);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var query = new GetJournalsQuery
        {
            PageNumber = page,
            PageSize = pageSize
        };

        var result = await _mediator.Send(query);

        return Ok(result);
    }
}