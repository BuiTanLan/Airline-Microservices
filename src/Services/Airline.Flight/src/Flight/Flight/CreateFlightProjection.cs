using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.EventStoreDB.Events;
using BuildingBlocks.EventStoreDB.Projections;
using Flight.Data;
using Flight.Flight.Events.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Flight.Flight;

public class FlightProjection : IProjection
{
    private readonly FlightDbContext _flightDbContext;

    public FlightProjection(FlightDbContext flightDbContext)
    {
        _flightDbContext = flightDbContext;
    }

    public async Task ProcessEventAsync<T>(StreamEvent<T> streamEvent, CancellationToken cancellationToken = default)
        where T : INotification
    {
        switch (streamEvent.Data)
        {
            case FlightCreatedDomainEvent flightCreatedDomainEvent:
                await Apply(flightCreatedDomainEvent, cancellationToken);
                break;
            case FlightUpdatedDomainEvent flightUpdatedDomainEvent:
                await Apply(flightUpdatedDomainEvent, cancellationToken);
                break;
        }
    }

    private async Task Apply(FlightCreatedDomainEvent @event, CancellationToken cancellationToken = default)
    {
        var flight = await _flightDbContext.Flights.SingleOrDefaultAsync(x => x.Id == @event.Id, cancellationToken);

        if (flight == null)
        {
            var model = Models.Flight.Create(@event.Id, @event.FlightNumber, @event.AircraftId, @event.DepartureAirportId,
                @event.DepartureDate, @event.ArriveDate, @event.ArriveAirportId, @event.DurationMinutes,
                @event.FlightDate,
                @event.Status, @event.Price);

            await _flightDbContext.Set<Models.Flight>().AddAsync(model, cancellationToken);
            await _flightDbContext.SaveChangesAsync(cancellationToken);
        }
    }

    private async Task Apply(FlightUpdatedDomainEvent @event, CancellationToken cancellationToken = default)
    {
        var flight = await _flightDbContext.Flights.SingleOrDefaultAsync(x => x.Id == @event.Id, cancellationToken);

        if (flight != null)
        {
            flight.Update(@event.Id, @event.FlightNumber, @event.AircraftId, @event.DepartureAirportId,
                @event.DepartureDate, @event.ArriveDate, @event.ArriveAirportId, @event.DurationMinutes,
                @event.FlightDate,
                @event.Status, @event.Price);

            _flightDbContext.Set<Models.Flight>().Update(flight);
            await _flightDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
