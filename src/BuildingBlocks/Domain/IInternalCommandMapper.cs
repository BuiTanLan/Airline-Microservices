using BuildingBlocks.Domain.Event;
using BuildingBlocks.InternalProcessor;

namespace BuildingBlocks.Domain;

public interface IInternalCommandMapper
{
    IInternalCommand Map(IDomainEvent @event);
    IEnumerable<IInternalCommand> MapAll(IEnumerable<IDomainEvent> events);
}
