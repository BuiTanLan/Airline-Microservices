namespace BuildingBlocks.EventStoreDB.Events.External;

public interface IExternalEventProducer
{
    Task Publish(IExternalEvent @event, CancellationToken ct);
}
