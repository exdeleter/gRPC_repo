using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Core;

public class RequestProcessingWorker : BackgroundService
{
    private readonly IRequestQueue _queue;
    private readonly IServiceScopeFactory _scopeFactory;
    // private readonly ILogger<RequestProcessingWorker> _logger;

    public RequestProcessingWorker(
        IRequestQueue queue, 
        IServiceScopeFactory scopeFactory
        // ILogger<RequestProcessingWorker> logger
        )
    {
        _queue = queue;
        _scopeFactory = scopeFactory;
        // _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // _logger.LogInformation("Request processing worker started");

        using var scope = _scopeFactory.CreateScope();
        var processor = scope.ServiceProvider.GetRequiredService<IRequestProcessor>();
        
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var dto = await _queue.DequeueAsync(stoppingToken);
                await processor.ProcessAsync(dto, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                // graceful shutdown
            }
            catch (Exception ex)
            {
                // _logger.LogError(ex, "Worker loop error");
            }
        }

        // _logger.LogInformation("Request processing worker stopped");
    }
}