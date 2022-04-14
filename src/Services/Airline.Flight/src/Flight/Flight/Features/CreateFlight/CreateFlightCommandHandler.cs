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

public class CreateFlightCommandHandler : IRequestHandler<CreateFlightCommand, FlightResponseDto>
{
    private readonly IEventStoreDBRepository<Models.Flight, long> _eventStoreDbRepository;

    public CreateFlightCommandHandler(IEventStoreDBRepository<Models.Flight, long> eventStoreDbRepository)
    {
        _eventStoreDbRepository = eventStoreDbRepository;
    }
    public async Task<FlightResponseDto> Handle(CreateFlightCommand command, CancellationToken cancellationToken)
    {

        return null;
    }
}
