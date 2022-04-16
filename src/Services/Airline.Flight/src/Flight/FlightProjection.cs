using System;
using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.EventStoreDB.Events;
using BuildingBlocks.EventStoreDB.Projections;
using Flight.Aircrafts.Events;
using Flight.Aircrafts.Models;
using Flight.Airports.Events;
using Flight.Airports.Models;
using Flight.Data;
using Flight.Flights.Events.Domain;
using Flight.Seats.Events;
using Flight.Seats.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Flight;

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
            case AirportCreatedDomainEvent airportCreatedDomainEvent:
                await Apply(airportCreatedDomainEvent, cancellationToken);
                break;
            case AircraftCreatedDomainEvent aircraftCreatedDomainEvent:
                await Apply(aircraftCreatedDomainEvent, cancellationToken);
                break;
            case FlightCreatedDomainEvent flightCreatedDomainEvent:
                await Apply(flightCreatedDomainEvent, cancellationToken);
                break;
            case FlightUpdatedDomainEvent flightUpdatedDomainEvent:
                await Apply(flightUpdatedDomainEvent, cancellationToken);
                break;
            case SeatCreatedDomainEvent seatCreatedDomainEvent:
                await Apply(seatCreatedDomainEvent, cancellationToken);
                break;
        }
    }

    private async Task Apply(FlightCreatedDomainEvent @event, CancellationToken cancellationToken = default)
    {
        var flight =
            await _flightDbContext.Flights.SingleOrDefaultAsync(x => x.Id == @event.Id && !x.IsDeleted,
                cancellationToken);

        if (flight == null)
        {
            var model = Flights.Models.Flight.Create(@event.Id, @event.FlightNumber, @event.AircraftId,
                @event.DepartureAirportId,
                @event.DepartureDate, @event.ArriveDate, @event.ArriveAirportId, @event.DurationMinutes,
                @event.FlightDate,
                @event.Status, @event.Price);

            await _flightDbContext.Set<Flights.Models.Flight>().AddAsync(model, cancellationToken);
            await _flightDbContext.SaveChangesAsync(cancellationToken);
        }
    }

    private async Task Apply(FlightUpdatedDomainEvent @event, CancellationToken cancellationToken = default)
    {
        var flight =
            await _flightDbContext.Flights.SingleOrDefaultAsync(x => x.Id == @event.Id && !x.IsDeleted,
                cancellationToken);

        if (flight != null)
        {
            flight.Update(@event.Id, @event.FlightNumber, @event.AircraftId, @event.DepartureAirportId,
                @event.DepartureDate, @event.ArriveDate, @event.ArriveAirportId, @event.DurationMinutes,
                @event.FlightDate,
                @event.Status, @event.Price);

            _flightDbContext.Set<Flights.Models.Flight>().Update(flight);
            await _flightDbContext.SaveChangesAsync(cancellationToken);
        }
    }

    private async Task Apply(AirportCreatedDomainEvent @event, CancellationToken cancellationToken = default)
    {
        var airport =
            await _flightDbContext.Airports.SingleOrDefaultAsync(x => x.Id == @event.Id && !x.IsDeleted,
                cancellationToken);

        if (airport == null)
        {
            var model = Airport.Create(@event.Id, @event.Name, @event.Address, @event.Code);

            await _flightDbContext.Set<Airport>().AddAsync(model, cancellationToken);
            await _flightDbContext.SaveChangesAsync(cancellationToken);
        }
    }


    private async Task Apply(AircraftCreatedDomainEvent @event, CancellationToken cancellationToken = default)
    {
        var aircraft =
            await _flightDbContext.Aircraft.SingleOrDefaultAsync(x => x.Id == @event.Id && !x.IsDeleted,
                cancellationToken);

        if (aircraft == null)
        {
            var model = Aircraft.Create(@event.Id, @event.Name, @event.Model, @event.ManufacturingYear);

            await _flightDbContext.Set<Aircraft>().AddAsync(model, cancellationToken);
            await _flightDbContext.SaveChangesAsync(cancellationToken);
        }
    }


    private async Task Apply(SeatCreatedDomainEvent @event, CancellationToken cancellationToken = default)
    {
        var seat = await _flightDbContext.Seats.SingleOrDefaultAsync(x => x.Id == @event.Id && !x.IsDeleted,
            cancellationToken);

        if (seat == null)
        {
            var model = Seat.Create(@event.Id, @event.SeatNumber, @event.Type, @event.Class, @event.FlightId);

            await _flightDbContext.Set<Seat>().AddAsync(model, cancellationToken);
            await _flightDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
