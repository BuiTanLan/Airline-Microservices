namespace BuildingBlocks.EventStoreDB.Events.External;

public interface IExternalEventConsumer
{
    Task StartAsync(CancellationToken cancellationToken);
}