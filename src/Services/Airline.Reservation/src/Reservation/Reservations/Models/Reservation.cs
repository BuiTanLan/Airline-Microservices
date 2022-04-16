using BuildingBlocks.Domain.Model;
using BuildingBlocks.IdsGenerator;
using Reservation.Reservations.Models.ValueObjects;

namespace Reservation.Reservations.Models;

public class Reservation : Aggregate<long>
{
    public Reservation()
    {
    }

    public static Reservation Create(PassengerInfo passengerInfo, Trip trip, long? id = null)
    {
        var reservation = new Reservation()
        {
            Id = id ?? SnowFlakIdGenerator.NewId(), Trip = trip, PassengerInfo = passengerInfo
        };

        return reservation;
    }

    public Trip Trip { get; private set; }
    public PassengerInfo PassengerInfo { get; private set; }
}
