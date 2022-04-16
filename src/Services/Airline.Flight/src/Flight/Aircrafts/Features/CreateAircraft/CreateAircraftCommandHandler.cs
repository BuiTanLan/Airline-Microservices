using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using BuildingBlocks.EventStoreDB.Repository;
using Flight.Aircrafts.Dtos;
using Flight.Aircrafts.Exceptions;
using Flight.Aircrafts.Models;
using Flight.Data;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Flight.Aircrafts.Features.CreateAircraft;

public class CreateAircraftCommandHandler : IRequestHandler<CreateAircraftCommand, ulong>
{
    private readonly IEventStoreDBRepository<Aircraft> _eventStoreDbRepository;

    public CreateAircraftCommandHandler(IEventStoreDBRepository<Aircraft> eventStoreDbRepository)
    {
        _eventStoreDbRepository = eventStoreDbRepository;
    }

    public async Task<ulong> Handle(CreateAircraftCommand command, CancellationToken cancellationToken)
    {
        Guard.Against.Null(command, nameof(command));

        var aircraft = await _eventStoreDbRepository.Find(command.Id, cancellationToken);

        if (aircraft is not null && !aircraft.IsDeleted)
            throw new AircraftAlreadyExistException();

        var aggrigate = Aircraft.Create(command.Id, command.Name, command.Model, command.ManufacturingYear);

        var result = await _eventStoreDbRepository.Add(
            aggrigate,
            cancellationToken);

        return result;
    }
}
