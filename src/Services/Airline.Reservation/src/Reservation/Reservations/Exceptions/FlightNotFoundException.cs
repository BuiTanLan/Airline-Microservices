using BuildingBlocks.Exception;

namespace Reservation.Reservations.Exceptions;

public class FlightNotFoundException : NotFoundException
{
    public FlightNotFoundException() : base("Flight doesn't exist!")
    {
    }
}
