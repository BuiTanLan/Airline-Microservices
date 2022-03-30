using BuildingBlocks.Exception;

namespace Reservation.Reservation.Exceptions;

public class FlightNotFoundException : NotFoundException
{
    public FlightNotFoundException() : base("Flight doesn't exist!")
    {
    }
}
