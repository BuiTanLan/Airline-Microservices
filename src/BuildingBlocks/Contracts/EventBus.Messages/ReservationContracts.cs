using BuildingBlocks.Domain.Event;

namespace BuildingBlocks.Contracts.EventBus.Messages;

public record ReservationCreated(long Id) : IIntegrationEvent;
