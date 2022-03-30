using BuildingBlocks.Domain.Event;

namespace BuildingBlocks.EventBus.Messages.Contracts;

public record FlightCreated(string FlightNumber) : IIntegrationEvent;
