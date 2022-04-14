using System;
using BuildingBlocks.Domain.Event;
using Flight.Flight.Models;

namespace Flight.Flight.Events.Domain;

public record FlightCreatedDomainEvent(long Id, string FlightNumber, long AircraftId, DateTime DepartureDate,
    long DepartureAirportId, DateTime ArriveDate, long ArriveAirportId, decimal DurationMinutes,
    DateTime FlightDate, FlightStatus Status, decimal Price) : IDomainEvent;
