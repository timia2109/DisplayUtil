
using System.Diagnostics;

namespace DisplayUtil.Utils;

/// <summary>
/// Helper class to handle scheduled background tasks
/// </summary>
public abstract partial class TimedScopedService(
    IServiceScopeFactory scopeFactory,
    ILogger logger
) : IHostedService
{
    private readonly ILogger _logger = logger;
    private CancellationTokenSource _cancellationTokenSource = new();

    protected abstract TimeSpan InitTimeout { get; }
    protected abstract TimeSpan Delay { get; }
    protected CancellationToken CancellationToken => _cancellationTokenSource.Token;

    protected abstract Task TriggerAsync(IServiceProvider serviceProvider);

    private async Task RunJobAsync()
    {
        LogTrigger();
        await using var scope = scopeFactory.CreateAsyncScope();
        var stopwatch = Stopwatch.StartNew();
        try
        {
            await TriggerAsync(scope.ServiceProvider);
            stopwatch.Stop();
            LogTriggered(stopwatch.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            LogError(ex);
        }
    }

    private async Task RunAsync(CancellationToken cancellationToken)
    {
        await Task.Delay(InitTimeout, cancellationToken);

        while (!cancellationToken.IsCancellationRequested)
        {
            await RunJobAsync();
            await Task.Delay(Delay, cancellationToken);
        }
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _ = RunAsync(_cancellationTokenSource.Token);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _cancellationTokenSource?.Cancel();
        return Task.CompletedTask;
    }

    [LoggerMessage(LogLevel.Information, "Trigger task")]
    private partial void LogTrigger();

    [LoggerMessage(LogLevel.Information, "Successfully triggered in {ms}ms")]
    private partial void LogTriggered(long ms);

    [LoggerMessage(LogLevel.Error, "Error on trigger")]
    private partial void LogError(Exception ex);
}