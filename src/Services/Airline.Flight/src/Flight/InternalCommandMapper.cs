using System.Collections.Generic;
using System.Linq;
using BuildingBlocks.Domain;
using BuildingBlocks.Domain.Event;
using Flight.Flights.Events.Domain;
using Flight.Flights.Features.CreateFlight.Read;

namespace Flight;

public class InternalCommandMapper : IInternalCommandMapper
{
    public IEnumerable<IInternalCommand> MapAll(IEnumerable<IDomainEvent> events) => events.Select(Map);

    public IInternalCommand Map(IDomainEvent @event)
    {
        return @event switch
        {
            FlightCreatedDomainEvent e => new CreateFlightMongoReadModel(e.FlightNumber, e.AircraftId, e.DepartureDate, e.DepartureAirportId,
                e.ArriveDate, e.ArriveAirportId, e.DurationMinutes, e.FlightDate, e.Status, e.Price),
            _ => null
        };
    }
}
