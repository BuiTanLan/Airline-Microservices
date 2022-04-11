namespace BuildingBlocks.EventStoreDB.Events.NoMediator;

public interface INoMediatorEventHandler<in TEvent>
{
    Task Handle(TEvent @event, CancellationToken ct);
}
