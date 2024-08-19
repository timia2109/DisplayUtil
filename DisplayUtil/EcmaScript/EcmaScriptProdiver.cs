using DisplayUtil.Layouting;
using DisplayUtil.Scenes;
using DisplayUtil.XmlModel;
using DisplayUtil.XmlModel.Models;
using Jint;
using Jint.Native.Function;
using SkiaSharp;

namespace DisplayUtil.EcmaScript;

internal class EcmaScriptProvider(
    DrawManager drawManager,
    Engine engine,
    Function func) : IScreenProvider
{
    public Task<SKBitmap> GetImageAsync()
    {
        var result = engine.Invoke(func);
        var screen = result.ToObject() as Screen
            ?? throw new ArgumentException("Module MUST return a Screen Instance");

        return Task.FromResult(
            drawManager.Draw(
                new SKSize(screen.Width, screen.Height),
                screen.AsElement(DefaultDefinition.Default)
            )
        );
    }
}

internal partial class EcmaScriptScreenProviderRepo(DrawManager drawManager,
    Engine engine,
    ILogger<EcmaScriptScreenProviderRepo> logger
) : IScreenProviderSource
{

    private readonly ILogger _logger = logger;

    public IScreenProvider? GetScreenProvider(string id)
    {
        try
        {
            var module = engine.Modules.Import(id);
            if (module is null) return null;

            if (module.Get("render") is not Function renderFunction) return null;

            return new EcmaScriptProvider(drawManager, engine, renderFunction);
        }
        catch (Exception ex)
        {
            LogExecutingError(id, ex);
            return null;
        }

    }

    [LoggerMessage(LogLevel.Error, "Error executing script {id}")]
    private partial void LogExecutingError(string id, Exception ex);
}