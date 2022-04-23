using System.Security.Claims;
using Ardalis.GuardClauses;
using BuildingBlocks.Domain.Event;
using BuildingBlocks.Outbox.EF;
using BuildingBlocks.Web;
using Humanizer;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace BuildingBlocks.Outbox.InMemory;

public class InMemoryOutboxService : IOutboxService
{
    private readonly IInMemoryOutboxStore _inMemoryOutboxStore;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<InMemoryOutboxService> _logger;
    private readonly OutboxOptions _options;
    private readonly IPublishEndpoint _pushEndpoint;

    public InMemoryOutboxService(
        IOptions<OutboxOptions> options,
        ILogger<InMemoryOutboxService> logger,
        IInMemoryOutboxStore inMemoryOutboxStore,
        IHttpContextAccessor httpContextAccessor,
        IPublishEndpoint pushEndpoint)
    {
        _options = options.Value;
        _logger = logger;
        _inMemoryOutboxStore = inMemoryOutboxStore;
        _httpContextAccessor = httpContextAccessor;
        _pushEndpoint = pushEndpoint;
    }

    public Task<IEnumerable<OutboxMessage>> GetAllUnsentOutboxMessagesAsync(
        EventType eventType = EventType.IntegrationEvent,
        CancellationToken cancellationToken = default)
    {
        var messages = _inMemoryOutboxStore.Events
            .Where(x => x.EventType == eventType && x.ProcessedOn == null);

        return Task.FromResult(messages);
    }

    public Task<IEnumerable<OutboxMessage>> GetAllOutboxMessagesAsync(
        EventType eventType = EventType.IntegrationEvent | EventType.DomainEvent,
        CancellationToken cancellationToken = default)
    {
        var messages = _inMemoryOutboxStore.Events
            .Where(x => x.EventType == eventType);

        return Task.FromResult(messages);
    }

    public Task CleanProcessedAsync(CancellationToken cancellationToken = default)
    {
        _inMemoryOutboxStore.Events.ToList().RemoveAll(x => x.ProcessedOn != null);

        return Task.CompletedTask;
    }


    public async Task PublishUnsentOutboxMessagesAsync(CancellationToken cancellationToken = default)
    {
        var unsentMessages = _inMemoryOutboxStore.Events
            .Where(x => x.ProcessedOn == null).ToList();

        if (!unsentMessages.Any()) _logger.LogTrace("No unsent messages found in outbox");

        _logger.LogTrace(
            "Found {Count} unsent messages in outbox, sending...",
            unsentMessages.Count);

        foreach (var outboxMessage in unsentMessages)
        {
            var type = Type.GetType(outboxMessage.Type);

            if (type is null)
                continue;

            dynamic data = JsonConvert.DeserializeObject(outboxMessage.Data, type);
            if (data is null)
            {
                _logger.LogError("Invalid message type: {Name}", type?.Name);
                continue;
            }

            if (outboxMessage.EventType == EventType.IntegrationEvent)
            {
                var integrationEvent = data as IIntegrationEvent;

                // integration event
                if (integrationEvent != null)
                {
                    await _pushEndpoint.Publish((object)integrationEvent, context =>
                    {
                        context.CorrelationId = outboxMessage.CorrelationId;
                        context.Headers.Set("UserId",
                            _httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier));
                        context.Headers.Set("UserName",
                            _httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.Name));
                    }, cancellationToken);

                    _logger.LogTrace(
                        "Publish a message: '{Name}' with ID: '{Id} (outbox)'",
                        outboxMessage.Name,
                        integrationEvent?.EventId);
                }
            }

            outboxMessage.MarkAsProcessed();
        }
    }

    public Task SaveAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(integrationEvent, nameof(integrationEvent));

        if (!_options.Enabled)
        {
            _logger.LogWarning("Outbox is disabled, outgoing messages won't be saved");
            return Task.CompletedTask;
        }

        var name = integrationEvent.GetType().Name;

        var outboxMessages = new OutboxMessage(
            integrationEvent.EventId,
            integrationEvent.OccurredOn,
            integrationEvent.EventType,
            name.Underscore(),
            JsonConvert.SerializeObject(integrationEvent),
            EventType.IntegrationEvent,
            new Guid(_httpContextAccessor.HttpContext.GetCorrelationId()));

        _inMemoryOutboxStore.Events.Add(outboxMessages);

        _logger.LogTrace("Saved message to the outbox");

        return Task.CompletedTask;
    }
}
