using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using BuildingBlocks.Domain;
using BuildingBlocks.EventStoreDB.Repository;
using Flight.Aircraft.Exceptions;
using Flight.Data;
using Flight.Flight.Dtos;
using Flight.Flight.Exceptions;
using Flight.Flight.Models;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Flight.Flight.Features.CreateFlight;

public class CreateFlightCommandHandler : IRequestHandler<CreateFlightCommand, ulong>
{
    private readonly IEventStoreDBRepository<Models.Flight, long> _eventStoreDbRepository;

    public CreateFlightCommandHandler(IEventStoreDBRepository<Models.Flight, long> eventStoreDbRepository)
    {
        _eventStoreDbRepository = eventStoreDbRepository;
    }
    public async Task<ulong> Handle(CreateFlightCommand command, CancellationToken cancellationToken)
    {
        Guard.Against.Null(command, nameof(command));

        var flight = await _eventStoreDbRepository.Find(command.Id, cancellationToken);

        if (flight is not null)
            throw new FlightAlreadyExistException();

        var aggrigate = Models.Flight.Create(command.Id, command.FlightNumber, command.AircraftId,
            command.DepartureAirportId, command.DepartureDate,
            command.ArriveDate, command.ArriveAirportId, command.DurationMinutes, command.FlightDate,
            FlightStatus.Completed, command.Price);

        var result = await _eventStoreDbRepository.Add(aggrigate, cancellationToken);

        return result;
    }
}
