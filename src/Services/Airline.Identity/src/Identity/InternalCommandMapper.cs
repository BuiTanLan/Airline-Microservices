using System.Collections.Generic;
using System.Linq;
using BuildingBlocks.Domain;
using BuildingBlocks.Domain.Event;

namespace Identity;

public class InternalCommandMapper : IInternalCommandMapper
{
    public IEnumerable<IInternalCommand> MapAll(IEnumerable<IDomainEvent> events) => events.Select(Map);

    public IInternalCommand Map(IDomainEvent @event)
    {
        return @event switch
        {
            _ => null
        };
    }
}
