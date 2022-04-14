using System.Collections.Immutable;
using BuildingBlocks.Domain.Event;

namespace BuildingBlocks.Domain.Model
{
    public abstract class BaseAggregateRoot<TId> : Entity<TId>, IAggregate<TId>
    {
        private readonly List<IDomainEvent> _domainEvents = new();
        public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        public void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public IEvent[] ClearDomainEvents()
        {
            var dequeuedEvents = _domainEvents.ToArray();

            _domainEvents.Clear();

            return dequeuedEvents;
        }

        public int Version { get; protected set;}

        public virtual void When(object @event) { }
    }
}
