﻿using BuildingBlocks.Domain.Model;
using BuildingBlocks.EventStoreDB.Events;
using BuildingBlocks.EventStoreDB.Serialization;
using EventStore.Client;

namespace BuildingBlocks.EventStoreDB.Repository;

public interface IEventStoreDBRepository<T> where T : class, IAggregate
{
    Task<T?> Find(Guid id, CancellationToken cancellationToken);
    Task<ulong> Add(T aggregate, CancellationToken cancellationToken);
    Task<ulong> Update(T aggregate, ulong? expectedRevision = null, CancellationToken cancellationToken = default);
    Task<ulong> Delete(T aggregate, ulong? expectedRevision = null, CancellationToken cancellationToken = default);
}

public class EventStoreDBRepository<T>: IEventStoreDBRepository<T> where T : class, IAggregate
{
    private readonly EventStoreClient eventStore;

    public EventStoreDBRepository(EventStoreClient eventStore)
    {
        this.eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
    }

    public Task<T?> Find(Guid id, CancellationToken cancellationToken) =>
        eventStore.AggregateStream<T>(
            id,
            cancellationToken
        );

    public async Task<ulong> Add(T aggregate, CancellationToken cancellationToken = default)
    {
        var result = await eventStore.AppendToStreamAsync(
            StreamNameMapper.ToStreamId<T>(aggregate.Id),
            StreamState.NoStream,
            GetEventsToStore(aggregate),
            cancellationToken: cancellationToken
        );
        return result.NextExpectedStreamRevision;
    }

    public async Task<ulong> Update(T aggregate, ulong? expectedRevision = null, CancellationToken cancellationToken = default)
    {
        var nextVersion = expectedRevision ?? (ulong)aggregate.Version;

        var result = await eventStore.AppendToStreamAsync(
            StreamNameMapper.ToStreamId<T>(aggregate.Id),
            nextVersion,
            GetEventsToStore(aggregate),
            cancellationToken: cancellationToken
        );
        return result.NextExpectedStreamRevision;
    }

    public Task<ulong> Delete(T aggregate, ulong? expectedRevision = null, CancellationToken cancellationToken = default) =>
        Update(aggregate, expectedRevision, cancellationToken);

    private static IEnumerable<EventData> GetEventsToStore(T aggregate)
    {
        var events = aggregate.ClearDomainEvents();

        return events
            .Select(EventStoreDBSerializer.ToJsonEventData);
    }
}
