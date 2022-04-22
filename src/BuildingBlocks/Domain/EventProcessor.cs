using System.Security.Claims;
using BuildingBlocks.Domain.Event;
using BuildingBlocks.InternalProcessor;
using BuildingBlocks.Outbox;
using BuildingBlocks.Web;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BuildingBlocks.Domain;

public sealed class EventProcessor : IEventProcessor
{
    private readonly IEventMapper _eventMapper;
    private readonly IInternalCommandMapper _internalCommandMapper;
    private readonly ILogger<IEventProcessor> _logger;
    private readonly IOutboxService _outboxService;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IInternalMessageService _internalMessageService;
    private readonly InternalProcessorOptions _internalProcessorOptions;
    private readonly OutboxOptions _outboxOptions;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public EventProcessor(IServiceScopeFactory serviceScopeFactory,
        IEventMapper eventMapper,
        IInternalCommandMapper internalCommandMapper,
        ILogger<IEventProcessor> logger,
        IOutboxService outboxService,
        IPublishEndpoint publishEndpoint,
        IHttpContextAccessor httpContextAccessor,
        IOptions<OutboxOptions> outboxOptions,
        IOptions<InternalProcessorOptions> internalProcessorOptions,
        IInternalMessageService internalMessageService)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _eventMapper = eventMapper;
        _internalCommandMapper = internalCommandMapper;
        _logger = logger;
        _outboxService = outboxService;
        _publishEndpoint = publishEndpoint;
        _httpContextAccessor = httpContextAccessor;
        _internalMessageService = internalMessageService;
        _internalProcessorOptions = internalProcessorOptions?.Value;
        _outboxOptions = outboxOptions?.Value;
    }

    public async Task ProcessAsync(IDomainEvent @event, CancellationToken cancellationToken = default) =>
        await ProcessAsync(new[] {@event}, cancellationToken).ConfigureAwait(false);

    public async Task ProcessAsync(IReadOnlyList<IDomainEvent> events, CancellationToken cancellationToken = default)
    {
        if (events is null) return;

        await ProcessIntegrationEvents(events, cancellationToken);

        await ProcessInternalCommands(events, cancellationToken);
    }

    public async Task PublishAsync(IIntegrationEvent @event, CancellationToken cancellationToken = default) =>
        await PublishAsync(new[] { @event }, cancellationToken).ConfigureAwait(false);

    public async Task SendInternalCommandAsync(IInternalCommand internalCommand, CancellationToken cancellationToken = default) =>
        await SendInternalCommandAsync(new[] { internalCommand }, cancellationToken).ConfigureAwait(false);

    public async Task SendInternalCommandAsync(IReadOnlyList<IInternalCommand> internalCommand, CancellationToken cancellationToken = default)
    {
        await _internalMessageService.SaveAsync(internalCommand, cancellationToken).ConfigureAwait(false);
    }

    public async Task PublishAsync(IReadOnlyList<IIntegrationEvent> integrationEvents,
        CancellationToken cancellationToken)
    {
        foreach (var integrationEvent in integrationEvents)
        {
            if (_outboxOptions.Enabled)
            {
                await _outboxService.SaveAsync(integrationEvent, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                await _publishEndpoint.Publish((object)integrationEvent, context =>
                {
                    context.CorrelationId = new Guid(_httpContextAccessor.HttpContext.GetCorrelationId());
                    context.Headers.Set("UserId",
                        _httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier));
                    context.Headers.Set("UserName",
                        _httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.Name));
                }, cancellationToken);

                _logger.LogTrace("Publish a message with ID: {Id}", integrationEvent?.EventId);
            }
        }
    }


    private async Task ProcessInternalCommands(IReadOnlyList<IDomainEvent> events, CancellationToken cancellationToken)
    {
        if (_internalProcessorOptions.Enabled)
        {
            _logger.LogTrace("Processing internal commands start...");

            var internalCommands = await MapDomainEventToInternalCommandAsync(events).ConfigureAwait(false);

            if (!internalCommands.Any()) return;

            await SendInternalCommandAsync(internalCommands, cancellationToken).ConfigureAwait(false);

            _logger.LogTrace("Processing internal commands done...");
        }
    }

    private async Task ProcessIntegrationEvents(IReadOnlyList<IDomainEvent> events, CancellationToken cancellationToken)
    {
        _logger.LogTrace("Processing integration events start...");

        var integrationEvents = await MapDomainEventToIntegrationEventAsync(events).ConfigureAwait(false);

        if (!integrationEvents.Any()) return;

        await PublishAsync(integrationEvents, cancellationToken).ConfigureAwait(false);

        _logger.LogTrace("Processing integration events done...");
    }

    private async Task<IReadOnlyList<IIntegrationEvent>> MapDomainEventToIntegrationEventAsync(
        IReadOnlyList<IDomainEvent> events)
    {
        var wrappedIntegrationEvents = GetWrappedIntegrationEvents(events.ToList())?.ToList();
        if (wrappedIntegrationEvents?.Count > 0)
            return wrappedIntegrationEvents;

        var integrationEvents = new List<IIntegrationEvent>();
        using var scope = _serviceScopeFactory.CreateScope();
        foreach (var @event in events)
        {
            var eventType = @event.GetType();
            _logger.LogTrace($"Handling domain event: {eventType.Name}");

            var integrationEvent = _eventMapper.Map(@event);

            if (integrationEvent is null) continue;

            integrationEvents.Add(integrationEvent);
        }

        return integrationEvents;
    }

    private async Task<IReadOnlyList<IInternalCommand>> MapDomainEventToInternalCommandAsync(
        IReadOnlyList<IDomainEvent> events)
    {
        var internalCommands = new List<IInternalCommand>();
        using var scope = _serviceScopeFactory.CreateScope();
        foreach (var @event in events)
        {
            var eventType = @event.GetType();
            _logger.LogTrace($"Handling domain event: {eventType.Name}");

            var internalCommand = _internalCommandMapper.Map(@event);

            if (internalCommand is null) continue;

            internalCommands.Add(internalCommand);
        }

        return internalCommands;
    }

    private IEnumerable<IIntegrationEvent> GetWrappedIntegrationEvents(IReadOnlyList<IDomainEvent> domainEvents)
    {
        foreach (var domainEvent in domainEvents.Where(x =>
                     x is IHaveIntegrationEvent))
        {
            var genericType = typeof(IntegrationEventWrapper<>)
                .MakeGenericType(domainEvent.GetType());

            var domainNotificationEvent = (IIntegrationEvent)Activator
                .CreateInstance(genericType, domainEvent);

            yield return domainNotificationEvent;
        }
    }
}
