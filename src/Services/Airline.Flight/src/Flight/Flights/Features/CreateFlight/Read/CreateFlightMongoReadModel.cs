using System;
using BuildingBlocks.Domain.Event;
using BuildingBlocks.InternalProcessor;
using Flight.Flights.Models;
using MediatR;

namespace Flight.Flights.Features.CreateFlight.Read;

public record CreateFlightMongoReadModel(string FlightNumber, long AircraftId, DateTime DepartureDate, long DepartureAirportId,
    DateTime ArriveDate, long ArriveAirportId, decimal DurationMinutes, DateTime FlightDate, FlightStatus Status, decimal Price) : IInternalCommand;
