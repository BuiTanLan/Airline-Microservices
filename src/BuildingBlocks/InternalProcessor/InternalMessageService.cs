using Ardalis.GuardClauses;
using BuildingBlocks.Domain.Event;
using BuildingBlocks.EFCore;
using BuildingBlocks.Web;
using Google.Protobuf;
using Humanizer;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace BuildingBlocks.InternalProcessor;

public class InternalMessageService : IInternalMessageService
{
    private readonly ILogger<InternalMessageService> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IDbContext _dbContext;
    private readonly IMediator _mediator;
    private readonly InternalProcessorOptions _internalProcessorOptions;

    public InternalMessageService(ILogger<InternalMessageService> logger,
        IOptions<InternalProcessorOptions> internalProcessorOptions,
        IHttpContextAccessor httpContextAccessor,
        IDbContext dbContext,
        IMediator mediator)
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
        _dbContext = dbContext;
        _mediator = mediator;
        _internalProcessorOptions = internalProcessorOptions?.Value;
    }

    public Task<IEnumerable<InternalMessage>> GetAllUnsentInternalMessagesAsync(
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<InternalMessage>> GetAllInternalMessagesAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task SaveAsync(IInternalCommand internalCommand, CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(internalCommand, nameof(internalCommand));

        await SaveAsync(new[] {internalCommand}, cancellationToken);
    }

    public async Task SaveAsync(IReadOnlyList<IInternalCommand> internalCommands, CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(internalCommands, nameof(internalCommands));

        if (!internalCommands.Any())
            return;

        if (!_internalProcessorOptions.Enabled)
        {
            _logger.LogWarning("Internal-Message is disabled, messages won't be saved");
            return;
        }

        foreach (var internalCommand in internalCommands)
        {
            string name = internalCommand.GetType().Name;

            var internalMessage = new InternalMessage(
                internalCommand.EventId,
                internalCommand.OccurredOn,
                internalCommand.CommandType,
                name.Underscore(),
                JsonConvert.SerializeObject(internalCommand),
                new Guid(_httpContextAccessor.HttpContext.GetCorrelationId()));

            await _dbContext.InternalMessages.AddAsync(internalMessage, cancellationToken);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogTrace("Saved message to the internal-messages store");
    }

    public async Task PublishUnsentInternalMessagesAsync(CancellationToken cancellationToken = default)
    {
        var unsentMessages = await _dbContext.InternalMessages
            .Where(x => x.ProcessedOn == null).ToListAsync(cancellationToken: cancellationToken);

        if (!unsentMessages.Any())
        {
            _logger.LogTrace("No unsent messages found in internal-messages store");
            return;
        }

        _logger.LogTrace(
            "Found {Count} unsent messages in internal-messages store, sending...",
            unsentMessages.Count);

        foreach (var internalMessage in unsentMessages)
        {
            var type = Type.GetType(internalMessage.CommandType);

            if (type is null)
                continue;

            dynamic data = JsonConvert.DeserializeObject(internalMessage.Data, type);
            if (data is null)
            {
                _logger.LogError("Invalid message type: {Name}", type?.Name);
                continue;
            }

            if (data is IInternalCommand internalCommand)
            {
                await _mediator.Send(internalCommand, cancellationToken);

                _logger.LogTrace(
                    "Sent a internal command: '{Name}' with ID: '{Id} (internal-message store)'",
                    internalMessage.Name,
                    internalCommand.EventId);
            }

            internalMessage.MarkAsProcessed();
        }

        await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}
