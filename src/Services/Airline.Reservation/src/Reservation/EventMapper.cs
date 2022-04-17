using BuildingBlocks.Contracts.EventBus.Messages;
using BuildingBlocks.Domain;
using BuildingBlocks.Domain.Event;
using Reservation.Reservations.Events.Domain;

namespace Reservation;

public sealed class EventMapper : IEventMapper
{
    public IEnumerable<IIntegrationEvent> MapAll(IEnumerable<IDomainEvent> events)
    {
        return events.Select(Map);
    }

    public IIntegrationEvent Map(IDomainEvent @event)
    {
        return @event switch
        {
            ReservationCreatedDomainEvent e => new ReservationCreated(e.Id),
            _ => null
        };
    }
}
