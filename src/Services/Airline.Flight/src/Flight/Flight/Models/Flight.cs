using System;
using BuildingBlocks.Domain;
using BuildingBlocks.Domain.Model;
using BuildingBlocks.IdsGenerator;
using Flight.Flight.Events.Domain;
using Flight.Flight.Exceptions;
using JetBrains.Annotations;

namespace Flight.Flight.Models;

public class Flight : BaseAggregateRoot
{
    public string FlightNumber { get; private set; }
    public long AircraftId { get; private set; }
    public DateTime DepartureDate { get; private set; }
    public long DepartureAirportId { get; private set; }
    public DateTime ArriveDate { get; private set; }
    public long ArriveAirportId { get; private set; }
    public decimal DurationMinutes { get; private set; }
    public DateTime FlightDate { get; private set; }
    public FlightStatus Status { get; private set; }
    public decimal Price { get; private set; }

    public static Flight Create(long id, string flightNumber, long aircraftId,
        long departureAirportId, DateTime departureDate, DateTime arriveDate,
        long arriveAirportId, decimal durationMinutes, DateTime flightDate, FlightStatus status,
        decimal price)
    {
        var flight = new Flight
        {
            Id = id,
            FlightNumber = flightNumber,
            AircraftId = aircraftId,
            DepartureAirportId = departureAirportId,
            DepartureDate = departureDate,
            ArriveDate = arriveDate,
            ArriveAirportId = arriveAirportId,
            DurationMinutes = durationMinutes,
            FlightDate = flightDate,
            Status = status,
            Price = price
        };

        var @event = new FlightCreatedDomainEvent(flight.Id, flight.FlightNumber, flight.AircraftId,
            flight.DepartureDate, flight.DepartureAirportId,
            flight.ArriveDate, flight.ArriveAirportId, flight.DurationMinutes, flight.FlightDate, flight.Status,
            flight.Price);

        flight.AddDomainEvent(@event);
        flight.Apply(@event);

        return flight;
    }


    public void Update(long id, string flightNumber, long aircraftId,
        long departureAirportId, DateTime departureDate, DateTime arriveDate,
        long arriveAirportId, decimal durationMinutes, DateTime flightDate, FlightStatus status,
        decimal price)
    {
        var @event = new FlightUpdatedDomainEvent(id, flightNumber, aircraftId, departureDate, departureAirportId,
            arriveDate, arriveAirportId, durationMinutes, flightDate, status, price);

        AddDomainEvent(@event);
        Apply(@event);
    }


    public override void When(object @event)
    {
        switch (@event)
        {
            case FlightCreatedDomainEvent flightCreated:
            {
                Apply(flightCreated);
                return;
            }

            case FlightUpdatedDomainEvent flightUpdated:
            {
                Apply(flightUpdated);
                return;
            }
        }
    }

    private void Apply(FlightCreatedDomainEvent @event)
    {
        Id = @event.Id;
        FlightNumber = @event.FlightNumber;
        Price = @event.Price;
        Status = @event.Status;
        AircraftId = @event.AircraftId;
        DepartureAirportId = @event.DepartureAirportId;
        DepartureDate = @event.DepartureDate;
        ArriveAirportId = @event.ArriveAirportId;
        ArriveDate = @event.ArriveDate;
        DurationMinutes = @event.DurationMinutes;
        FlightDate = @event.FlightDate;
        Version++;
    }

    private void Apply(FlightUpdatedDomainEvent @event)
    {
        Id = @event.Id;
        FlightNumber = @event.FlightNumber;
        Price = @event.Price;
        Status = @event.Status;
        AircraftId = @event.AircraftId;
        DepartureAirportId = @event.DepartureAirportId;
        DepartureDate = @event.DepartureDate;
        ArriveAirportId = @event.ArriveAirportId;
        ArriveDate = @event.ArriveDate;
        DurationMinutes = @event.DurationMinutes;
        FlightDate = @event.FlightDate;
        Version++;
    }
}
