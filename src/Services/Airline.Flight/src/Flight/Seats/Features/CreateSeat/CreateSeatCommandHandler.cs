using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using BuildingBlocks.EventStoreDB.Repository;
using Flight.Airports.Exceptions;
using Flight.Airports.Features.CreateAirport;
using Flight.Airports.Models;
using Flight.Seats.Exceptions;
using Flight.Seats.Models;
using MediatR;

namespace Flight.Seats.Features.CreateSeat;

public class CreateSeatCommandHandler : IRequestHandler<CreateSeatCommand, ulong>
{
    private readonly IEventStoreDBRepository<Seat> _eventStoreDbRepository;

    public CreateSeatCommandHandler(IEventStoreDBRepository<Seat> eventStoreDbRepository)
    {
        _eventStoreDbRepository = eventStoreDbRepository;
    }

    public async Task<ulong> Handle(CreateSeatCommand command, CancellationToken cancellationToken)
    {
        Guard.Against.Null(command, nameof(command));

        var seat = await _eventStoreDbRepository.Find(command.Id, cancellationToken);

        if (seat is not null && !seat.IsDeleted)
            throw new SeatAlreadyExistException();

        var aggrigate = Seat.Create(command.Id, command.SeatNumber, command.Type, command.Class, command.FlightId);

        var result = await _eventStoreDbRepository.Add(
            aggrigate,
            cancellationToken);

        return result;
    }
}
