using BuildingBlocks.EventStoreDB.Events;
using MediatR;

namespace BuildingBlocks.EventStoreDB.Projections;

public interface IProjection
{
    Task ProcessEventAsync<T>(StreamEvent<T> streamEvent, CancellationToken cancellationToken = default)
        where T : INotification;
}
