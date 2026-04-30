using Journal.Api.Models;
using Journal.Application.DTOs;
using Journal.Domain.Abstractions;
using Journal.Infrastructure.MessageBus;
using Journal.Infrastructure.MessageBus.Queues;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Journal.Api.Controllers;

[ApiController]
[Route("api/journal")]
public class JournalController : Controller
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly IPublisher<JournalMessage> _journalPublisher;
    private readonly IJournalRepository _journalRepository;
    private readonly IUnitOfWork _unitOfWork;

    public JournalController(
        IUnitOfWork unitOfWork,
        IJournalRepository journalRepository,
        IPublisher<JournalMessage> journalPublisher,
        IConnectionMultiplexer connectionMultiplexer)
    {
        _unitOfWork = unitOfWork;
        _journalRepository = journalRepository;
        _journalPublisher = journalPublisher;
        _connectionMultiplexer = connectionMultiplexer;
    }

    [HttpPost("")]
    public async Task<IActionResult> Add([FromBody] JournalAddRequest journalRequest)
    {
        var journalMessage = journalRequest.ToModel();

        await _journalPublisher.SendMessageAsync(journalMessage, QueuesName.JournalQueue);

        return Ok(journalMessage.Id);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromBody] JournalAddRequest journalRequest)
    {
        throw new NotImplementedException();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var redisDb = _connectionMultiplexer.GetDatabase();

        var keyName = $"{nameof(JournalResponse)}-{id}";

        string response = await redisDb.StringGetAsync(keyName);

        if (string.IsNullOrEmpty(response))
        {
            var result = await _journalRepository.GetByIdAsync(id);

            if (result is not null)
            {
                response = JsonConvert.SerializeObject(result);
            }

            await redisDb.StringSetAsync(keyName, response, TimeSpan.FromMinutes(1));
        }

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remove(Guid id)
    {
        var result = await _journalRepository.RemoveAsync(id);

        if (!result) return NotFound();
        await _unitOfWork.CommitAsync();
        return Ok();
    }
}