using BuildingBlocks.IdsGenerator;
using Flight.Seats.Models;
using MediatR;

namespace Flight.Seats.Features.CreateSeat;

public record CreateSeatCommand(string SeatNumber, SeatType Type, SeatClass Class, long FlightId) : IRequest<ulong>
{
    public long Id { get; set; } = SnowFlakIdGenerator.NewId();
}
