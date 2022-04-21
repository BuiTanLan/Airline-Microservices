using MediatR;

namespace BuildingBlocks.Domain.Event;

public interface IInternalCommand : INotification, IRequest<Unit>
{
    Guid Id => Guid.NewGuid();
    public DateTime OccurredOn => DateTime.Now;
    public string CommandType => GetType().AssemblyQualifiedName;
}
