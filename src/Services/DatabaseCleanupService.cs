using EvApplicationApi.Repositories.Interfaces;

public class DatabaseCleanupService : IHostedService, IDisposable
{
    private readonly ILogger<DatabaseCleanupService> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private int executionCount = 0;
    private Timer? _timer = null;

    public DatabaseCleanupService(
        ILogger<DatabaseCleanupService> logger,
        IServiceScopeFactory serviceScopeFactory
    )
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Database Cleanup Service running.");
        _timer = new Timer(DatabaseCleanup, null, TimeSpan.Zero, TimeSpan.FromMinutes(15));
        return Task.CompletedTask;
    }

    private async void DatabaseCleanup(object? state)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var applicationRepository =
                scope.ServiceProvider.GetRequiredService<IApplicationRepository>();

            var fileUploadRepository =
                scope.ServiceProvider.GetRequiredService<IFileUploadRepository>();

            var incompleteApplications =
                await applicationRepository.GetAllExpiredIncompleteApplications();
            foreach (var application in incompleteApplications)
            {
                await fileUploadRepository.DeleteAllFilesInApplication(application.ReferenceNumber);
                await applicationRepository.DeleteApplication(application.ReferenceNumber);
            }
        }

        var count = Interlocked.Increment(ref executionCount);
        _logger.LogInformation($"Database Cleanup Service is working. {count}");
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Database Cleanup Service is stopping.");
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
