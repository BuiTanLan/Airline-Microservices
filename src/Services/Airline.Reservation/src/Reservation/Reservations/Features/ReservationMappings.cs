using Mapster;
using Reservation.Reservations.Dtos;

namespace Reservation.Reservations.Features;

public class ReservationMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.Default.NameMatchingStrategy(NameMatchingStrategy.Flexible);

        config.NewConfig<Models.Reservation, ReservationResponseDto>()
            .Map(d => d.Name, s => s.PassengerInfo.Name)
            .Map(d => d.Description, s => s.Trip.Description)
            .Map(d => d.DepartureAirportId, s => s.Trip.DepartureAirportId)
            .Map(d => d.ArriveAirportId, s => s.Trip.ArriveAirportId)
            .Map(d => d.FlightNumber, s => s.Trip.FlightNumber)
            .Map(d => d.FlightDate, s => s.Trip.FlightDate)
            .Map(d => d.Price, s => s.Trip.Price)
            .Map(d => d.SeatNumber, s => s.Trip.SeatNumber)
            .Map(d => d.AircraftId, s => s.Trip.AircraftId);
    }
}

