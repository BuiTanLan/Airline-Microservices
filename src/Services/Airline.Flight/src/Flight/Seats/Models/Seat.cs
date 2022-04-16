using System;
using System.Threading.Tasks;
using BuildingBlocks.Domain.Model;
using BuildingBlocks.IdsGenerator;
using Flight.Airports.Events;
using Flight.Seats.Events;

namespace Flight.Seats.Models;

public class Seat : Aggregate<long>
{
    public string SeatNumber { get; private set; }
    public SeatType Type { get; private set; }
    public SeatClass Class { get; private set; }
    public long FlightId { get; private set; }

    public static Seat Create(long id, string seatNumber, SeatType type, SeatClass @class, long flightId)
    {
        var seat = new Seat()
        {
            Id = id,
            Class = @class,
            Type = type,
            SeatNumber = seatNumber,
            FlightId = flightId
        };

        var @event = new SeatCreatedDomainEvent(
            seat.Id,
            seat.SeatNumber,
            seat.Type,
            seat.Class,
            seat.FlightId);

        seat.AddDomainEvent(@event);
        seat.Apply(@event);


        return seat;
    }

    public Task<Seat> ReserveSeat(Seat seat)
    {
        seat.IsDeleted = true;
        seat.LastModified = DateTime.Now;
        return Task.FromResult(this);
    }

    public override void When(object @event)
    {
        switch (@event)
        {
            case SeatCreatedDomainEvent seatCreated:
            {
                Apply(seatCreated);
                return;
            }
        }
    }

    private void Apply(SeatCreatedDomainEvent @event)
    {
        Id = @event.Id;
        SeatNumber = @event.SeatNumber;
        Type = @event.Type;
        Class = @event.Class;
        FlightId = @event.FlightId;
        Version++;
    }
}
