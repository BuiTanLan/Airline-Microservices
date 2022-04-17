using BuildingBlocks.Exception;

namespace Reservation.Reservations.Exceptions;

public class ReservationAlreadyExistException : ConflictException
{
    public ReservationAlreadyExistException(string code = default) : base("Reservation already exist!", code)
    {
    }
}
