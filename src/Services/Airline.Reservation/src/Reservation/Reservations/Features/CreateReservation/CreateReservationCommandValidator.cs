using FluentValidation;

namespace Reservation.Reservations.Features.CreateReservation;

public class CreateReservationCommandValidator: AbstractValidator<CreateReservationCommand>
{
    public CreateReservationCommandValidator()
    {
        CascadeMode = CascadeMode.Stop;
        
        RuleFor(x => x.FlightId).NotNull().WithMessage("FlightId is required!");
        RuleFor(x => x.PassengerId).NotNull().WithMessage("PassengerId is required!");
    }
}