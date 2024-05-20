using DisplayUtil.Layouting;
using DisplayUtil.Scenes;
using DisplayUtil.XmlModel;
using DisplayUtil.XmlModel.Models;
using Esprima;
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

internal class EcmaScriptScreenProviderRepo(DrawManager drawManager, Engine engine) : IScreenProviderSource
{
    public IScreenProvider? GetScreenProvider(string id)
    {
        try
        {
            var module = engine.Modules.Import(id);
            if (module is null) return null;

            if (module.Get("render") is not Function renderFunction) return null;

            return new EcmaScriptProvider(drawManager, engine, renderFunction);
        }
        catch
        {
            return null;
        }

    }
}