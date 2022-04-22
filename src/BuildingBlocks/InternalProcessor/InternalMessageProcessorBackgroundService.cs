using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BuildingBlocks.InternalProcessor;

public class InternalMessageProcessorBackgroundService : BackgroundService
{
    private readonly bool _enabled;
    private readonly TimeSpan _interval;
    private readonly ILogger<InternalMessageProcessorBackgroundService> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public InternalMessageProcessorBackgroundService(
        IServiceScopeFactory serviceScopeFactory,
        IOptions<InternalProcessorOptions> internalProcessorOptions,
        ILogger<InternalMessageProcessorBackgroundService> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
        _interval = internalProcessorOptions.Value?.Interval ?? TimeSpan.FromSeconds(5);
        _enabled = internalProcessorOptions.Value?.Enabled ?? false;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!_enabled)
        {
            _logger.LogTrace("Internal message is disabled");
            return;
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogTrace("Started processing internal messages...");
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                try
                {
                    var internalMessageService = scope.ServiceProvider.GetRequiredService<IInternalMessageService>();
                    await internalMessageService.PublishUnsentInternalMessagesAsync(stoppingToken);
                }
                catch (System.Exception exception)
                {
                    _logger.LogError(
                        "There was an error when processing internal messages, exception is: {Exception}",
                        exception.Message);
                }
            }

            stopwatch.Stop();
            _logger.LogTrace(
                "Finished processing internal messages in {ElapsedMilliseconds} ms",
                stopwatch.ElapsedMilliseconds);
            await Task.Delay(_interval, stoppingToken);
        }
    }
}
