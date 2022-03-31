using BuildingBlocks.Domain.Event;

namespace BuildingBlocks.Contracts.EventBus.Messages;

public record FlightCreated(string FlightNumber) : IIntegrationEvent;
