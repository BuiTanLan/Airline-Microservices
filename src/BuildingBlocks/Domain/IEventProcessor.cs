using BuildingBlocks.Domain.Event;
using BuildingBlocks.InternalProcessor;

namespace BuildingBlocks.Domain;

public interface IEventProcessor
{
    Task ProcessAsync(IReadOnlyList<IDomainEvent> events, CancellationToken cancellationToken = default);
    Task ProcessAsync(IDomainEvent @event, CancellationToken cancellationToken = default);
    public Task PublishAsync(IReadOnlyList<IIntegrationEvent> events, CancellationToken cancellationToken = default);
    public Task PublishAsync(IIntegrationEvent @event, CancellationToken cancellationToken = default);
    public Task SendInternalCommandAsync(IInternalCommand internalCommand, CancellationToken cancellationToken = default);
    public Task SendInternalCommandAsync(IReadOnlyList<IInternalCommand> internalCommands, CancellationToken cancellationToken = default);
}
