using BuildingBlocks.Domain.Event;
using BuildingBlocks.EventStoreDB.Events;

namespace BuildingBlocks.Domain.Model
{
    public interface IAggregate : IAggregate<long>
    {
    }

    public interface IAggregate<out T> : IProjection
    {
        T Id { get; }
        IReadOnlyList<IDomainEvent> DomainEvents { get; }
        IEvent[] ClearDomainEvents();
        long Version { get; }
    }
}
