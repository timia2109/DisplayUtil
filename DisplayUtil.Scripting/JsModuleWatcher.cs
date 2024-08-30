using System.Globalization;
using DisplayUtil.EcmaScript;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace DisplayUtil.Scripting;

/// <summary>
/// Watches the registered paths for changes and disposes the engine
/// </summary>
internal class JsModuleWatcher(
    IOptions<JsSettings> options,
    EngineFactory engineFactory) : IHostedService
{
    private readonly List<FileSystemWatcher> _watchers = [];

    public Task StartAsync(CancellationToken cancellationToken)
    {
        foreach (var path in options.Value.Paths.Values)
        {
            var watcher = new FileSystemWatcher(path)
            {
                IncludeSubdirectories = true,
                NotifyFilter = NotifyFilters.LastWrite
            };
            watcher.Changed += OnChanged;
            watcher.EnableRaisingEvents = true;
            _watchers.Add(watcher);
        }

        engineFactory.CreateEngine(CultureInfo.CurrentCulture);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        foreach (var watcher in _watchers)
        {
            watcher.Dispose();
        }

        return Task.CompletedTask;
    }

    private async void OnChanged(object sender, FileSystemEventArgs e)
    {
        if (e.ChangeType != WatcherChangeTypes.Changed)
        {
            return;
        }

        if (!DisplayUtilModuleLoader._allowedExtensions
            .Any(a => Path.GetExtension(e.FullPath).EndsWith(a)))
        {
            return;
        }

        await Task.Delay(500);
        engineFactory.CreateEngine(CultureInfo.CurrentCulture);
    }
}