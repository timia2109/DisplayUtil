using System.Globalization;
using DisplayUtil.Scripting;
using Jint;

namespace DisplayUtil.EcmaScript;

public class EngineHandle : IDisposable
{
    public Engine Engine { get; }

    private Action _onDispose;

    internal EngineHandle(Engine engine, Action onDispose)
    {
        Engine = engine;
        _onDispose = onDispose;
    }

    public void Dispose()
    {
        _onDispose();
    }
}

internal class EngineProvider(
    EngineFactory engineFactory
)
{
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public EngineHandle GetEngineHandle()
    {
        _semaphore.Wait();

        var engine = engineFactory.CurrentEngine
            ?? throw new InvalidOperationException("Engine not created");

        return new EngineHandle(engine, () => _semaphore.Release());
    }
}