using BuildingBlocks.Domain.Event;
using Reservation.Reservations.Models.ValueObjects;

namespace Reservation.Reservations.Events.Domain;

public record ReservationCreatedDomainEvent(long Id, PassengerInfo PassengerInfo, Trip Trip) : IDomainEvent;
