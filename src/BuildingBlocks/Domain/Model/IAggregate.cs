using BuildingBlocks.Domain.Event;
using BuildingBlocks.EventStoreDB.Core.Projections;

namespace BuildingBlocks.Domain.Model
{
    public interface IAggregate : IAggregate<Guid>
    {
    }

    public interface IAggregate<out T> : IProjection
    {
        T Id { get; }
        IReadOnlyList<IDomainEvent> DomainEvents { get; }
        IEvent[] ClearDomainEvents();
        int Version { get; }
    }
}
