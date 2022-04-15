using System;
using BuildingBlocks.IdsGenerator;
using Flight.Flight.Models;
using MediatR;

namespace Flight.Flight.Features.UpdateFlight;

public record UpdateFlightCommand(long Id, string FlightNumber, long AircraftId, long DepartureAirportId,
    DateTime DepartureDate, DateTime ArriveDate, long ArriveAirportId,
    decimal DurationMinutes, DateTime FlightDate, FlightStatus Status, decimal Price) : IRequest<ulong>;
