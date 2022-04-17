using BuildingBlocks.IdsGenerator;
using MediatR;
using Reservation.Reservations.Dtos;

namespace Reservation.Reservations.Features.CreateReservation;

public record CreateReservationCommand
    (long PassengerId, long FlightId, string Description) : IRequest<ulong>
{
    public long Id { get; set; } = SnowFlakIdGenerator.NewId();
}

