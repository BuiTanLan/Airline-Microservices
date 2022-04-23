using BuildingBlocks.Domain.Model;
using BuildingBlocks.IdsGenerator;
using Reservation.Reservations.Events.Domain;
using Reservation.Reservations.Models.ValueObjects;

namespace Reservation.Reservations.Models;

public class Reservation : Aggregate<long>
{
    public Reservation()
    {
    }

    public Trip Trip { get; private set; }
    public PassengerInfo PassengerInfo { get; private set; }

    public static Reservation Create(long id, PassengerInfo passengerInfo, Trip trip, bool isDeleted = false)
    {
        var reservation = new Reservation()
        {
            Id = id,
            Trip = trip,
            PassengerInfo = passengerInfo,
            IsDeleted = isDeleted
        };

        var @event = new ReservationCreatedDomainEvent(reservation.Id, reservation.PassengerInfo, reservation.Trip, reservation.IsDeleted);

        reservation.AddDomainEvent(@event);
        reservation.Apply(@event);

        return reservation;
    }

    public override void When(object @event)
    {
        switch (@event)
        {
            case ReservationCreatedDomainEvent reservationCreated:
            {
                Apply(reservationCreated);
                return;
            }
        }
    }

    private void Apply(ReservationCreatedDomainEvent @event)
    {
        Id = @event.Id;
        Trip = @event.Trip;
        PassengerInfo = @event.PassengerInfo;
        IsDeleted = @event.IsDeleted;
        Version++;
    }
}
