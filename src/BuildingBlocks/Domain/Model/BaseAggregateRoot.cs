using System.Collections.Immutable;
using BuildingBlocks.Domain.Event;

namespace BuildingBlocks.Domain.Model
{
    public abstract class BaseAggregateRoot : BaseAggregateRoot<long>
    {
    }

    public abstract class BaseAggregateRoot<TId> : Entity, IAggregate
    {
        private readonly List<IDomainEvent> _domainEvents = new();
        public long Id { get; protected set; }
        public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        public void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public IEvent[] ClearDomainEvents()
        {
            IEvent[] dequeuedEvents = _domainEvents.ToArray();

            _domainEvents.Clear();

            return dequeuedEvents;
        }

        public long Version { get; protected set; } = -1;

        public virtual void When(object @event) { }
    }
}
