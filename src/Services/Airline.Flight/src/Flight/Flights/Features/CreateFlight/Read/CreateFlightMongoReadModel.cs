using System;
using BuildingBlocks.Domain.Event;
using BuildingBlocks.InternalProcessor;
using Flight.Flights.Models;
using MediatR;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Flight.Flights.Features.CreateFlight.Read;

public record CreateFlightMongoReadModel(long Id, string FlightNumber, long AircraftId, DateTime DepartureDate, long DepartureAirportId,
    DateTime ArriveDate, long ArriveAirportId, decimal DurationMinutes, DateTime FlightDate, FlightStatus Status, decimal Price) : IInternalCommand;

