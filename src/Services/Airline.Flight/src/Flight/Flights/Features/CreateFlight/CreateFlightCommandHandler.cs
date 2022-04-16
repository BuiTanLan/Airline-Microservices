using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using BuildingBlocks.EventStoreDB.Repository;
using Flight.Flights.Exceptions;
using Flight.Flights.Models;
using MediatR;

namespace Flight.Flights.Features.CreateFlight;

public class CreateFlightCommandHandler : IRequestHandler<CreateFlightCommand, ulong>
{
    private readonly IEventStoreDBRepository<Models.Flight> _eventStoreDbRepository;

    public CreateFlightCommandHandler(IEventStoreDBRepository<Models.Flight> eventStoreDbRepository)
    {
        _eventStoreDbRepository = eventStoreDbRepository;
    }

    public async Task<ulong> Handle(CreateFlightCommand command, CancellationToken cancellationToken)
    {
        Guard.Against.Null(command, nameof(command));

        var flight = await _eventStoreDbRepository.Find(command.Id, cancellationToken);

        if (flight is not null && !flight.IsDeleted)
            throw new FlightAlreadyExistException();

        var aggrigate = Models.Flight.Create(command.Id, command.FlightNumber, command.AircraftId,
            command.DepartureAirportId, command.DepartureDate,
            command.ArriveDate, command.ArriveAirportId, command.DurationMinutes, command.FlightDate,
            FlightStatus.Completed, command.Price);

        var result = await _eventStoreDbRepository.Add(
            aggrigate,
            cancellationToken);

        return result;
    }
}
