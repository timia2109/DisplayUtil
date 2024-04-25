using DisplayUtil.Scenes;
using DisplayUtil.Template;
using Esprima;
using Esprima.Utils;
using SkiaSharp;

namespace DisplayUtil.EcmaScript;

internal class EcmaScriptProvider : IScreenProvider
{
    private readonly JavaScriptParser _parser = new JsxParser();

    public Task<SKBitmap> GetImageAsync()
    {
        var source = """
            const a = 1+2;
            export const render = () => (
                <HBox>
                    <VBox>
                        <Text content="Hello World" />
                        <Text content={`1+2=${a}`} />
                    </VBox>
                </HBox>
            );
        """;

        var script = _parser.ParseModule(source);
        return Task.FromResult(new SKBitmap(1, 1));
    }
}

internal class EcmaScriptScreenProviderRepo : IScreenProviderSource
{
    public IScreenProvider? GetScreenProvider(string id)
    {
        if (id == "ecma") return new EcmaScriptProvider();
        return null;
    }
}