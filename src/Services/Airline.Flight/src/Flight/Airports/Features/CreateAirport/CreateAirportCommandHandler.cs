using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using BuildingBlocks.EventStoreDB.Repository;
using Flight.Airports.Dtos;
using Flight.Airports.Exceptions;
using Flight.Airports.Models;
using Flight.Data;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Flight.Airports.Features.CreateAirport;

public class CreateAirportCommandHandler : IRequestHandler<CreateAirportCommand, ulong>
{
    private readonly IEventStoreDBRepository<Airport> _eventStoreDbRepository;

    public CreateAirportCommandHandler(IEventStoreDBRepository<Airport> eventStoreDbRepository)
    {
        _eventStoreDbRepository = eventStoreDbRepository;
    }

    public async Task<ulong> Handle(CreateAirportCommand command, CancellationToken cancellationToken)
    {
        Guard.Against.Null(command, nameof(command));

        var airport = await _eventStoreDbRepository.Find(command.Id, cancellationToken);

        if (airport is not null && !airport.IsDeleted)
            throw new AirportAlreadyExistException();

        var aggrigate = Airport.Create(command.Id, command.Name, command.Address, command.Code);

        var result = await _eventStoreDbRepository.Add(
            aggrigate,
            cancellationToken);

        return result;
    }
}
