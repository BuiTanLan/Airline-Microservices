using BuildingBlocks.Domain.Event;

namespace BuildingBlocks.EventBus.Messages.Contracts;

public record UserCreated(long Id, string Name, string PassportNumber) : IIntegrationEvent;
