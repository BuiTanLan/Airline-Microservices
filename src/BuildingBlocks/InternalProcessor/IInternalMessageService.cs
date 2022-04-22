using BuildingBlocks.Domain.Event;
using Google.Protobuf;

namespace BuildingBlocks.InternalProcessor;

public interface IInternalMessageService
{
    Task<IEnumerable<InternalMessage>> GetAllUnsentInternalMessagesAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<InternalMessage>> GetAllInternalMessagesAsync(CancellationToken cancellationToken = default);
    Task SaveAsync(IInternalCommand internalCommand, CancellationToken cancellationToken = default);
    Task SaveAsync(IReadOnlyList<IInternalCommand> internalCommands, CancellationToken cancellationToken = default);
    Task PublishUnsentInternalMessagesAsync(CancellationToken cancellationToken = default);
}
