using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using BuildingBlocks.EventStoreDB.Repository;
using Flight.Flights.Exceptions;
using Flight.Flights.Models;
using MediatR;

namespace Flight.Flights.Features.UpdateFlight;

public class UpdateFlightCommandHandler : IRequestHandler<UpdateFlightCommand, ulong>
{
    private readonly IEventStoreDBRepository<Models.Flight> _eventStoreDbRepository;

    public UpdateFlightCommandHandler(IEventStoreDBRepository<Models.Flight> eventStoreDbRepository)
    {
        _eventStoreDbRepository = eventStoreDbRepository;
    }

    public async Task<ulong> Handle(UpdateFlightCommand command, CancellationToken cancellationToken)
    {
        Guard.Against.Null(command, nameof(command));

        var flight = await _eventStoreDbRepository.Find(command.Id, cancellationToken);

        if (flight is null)
            throw new FlightNotFountException();

        var flightVer = flight.Version;

        flight.Update(command.Id, command.FlightNumber, command.AircraftId,
            command.DepartureAirportId, command.DepartureDate,
            command.ArriveDate, command.ArriveAirportId, command.DurationMinutes, command.FlightDate,
            FlightStatus.Completed, command.Price);

        var result = await _eventStoreDbRepository.Update(flight, flightVer, cancellationToken);

        return result;
    }
}
