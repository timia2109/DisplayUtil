using DisplayUtil.Awtrix;
using NetDaemon.HassModel;

namespace DisplayUtil.Home.Awtrix;

public class AwtrixService(IServiceScopeFactory scopeFactory, IAwtrixClient client) : IHostedService
{
    private readonly List<IDisposable> _disposables = [];
    private IServiceScope _scope;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _scope = scopeFactory.CreateScope();

        Listen(ctx => ctx.Entity("input_select.tagesmodus")
            .StateChanges()
            .Subscribe(s =>
            {
                var newStateValue = s.New.State;
                client.SetPowerAsync(newStateValue is "Bett" or "Schlafvorbereitung");
                client.SetSettingsAsync(new AwtirxSettings
                {
                    Brightness = newStateValue == "Bett" ? 128 : 32
                });
            })
        );

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        foreach (var disposable in _disposables) disposable.Dispose();
        _scope.Dispose();

        return Task.CompletedTask;
    }

    private void Listen(Func<IHaContext, IDisposable> action)
    {
        var ctx = _scope.ServiceProvider.GetRequiredService<IHaContext>();
        var disposable = action(ctx);
        _disposables.Add(disposable);
    }
}