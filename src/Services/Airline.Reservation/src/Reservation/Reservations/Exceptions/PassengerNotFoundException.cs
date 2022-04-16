using BuildingBlocks.Exception;

namespace Reservation.Reservations.Exceptions;

public class PassengerNotFoundException: NotFoundException
{
    public PassengerNotFoundException() : base("Flight doesn't exist! ")
    {
    }
}