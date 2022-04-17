using System;
using Flight.Flights.Dtos;
using Flight.Flights.Models;
using MediatR;

namespace Flight.Flights.Features.UpdateFlight;

public record UpdateFlightCommand(long Id, string FlightNumber, long AircraftId, long DepartureAirportId,
    DateTime DepartureDate, DateTime ArriveDate, long ArriveAirportId,
    decimal DurationMinutes, DateTime FlightDate, FlightStatus Status, decimal Price) : IRequest<FlightResponseDto>;
