using DisplayUtil.Layouting;
using DisplayUtil.Scenes;
using DisplayUtil.Utils;
using DisplayUtil.XmlModel;
using Scriban.Parsing;
using Scriban.Runtime;
using SkiaSharp;

namespace DisplayUtil.Template;

internal class ScibanScreenProviderSource(IServiceProvider serviceProvider,
    TemplateLoader templateLoader
)
    : IScreenProviderSource
{
    private static readonly ObjectFactory<ScribanScreenProvider>
        factory = ActivatorUtilities.CreateFactory<ScribanScreenProvider>([
            typeof(string)
        ]);

    public IScreenProvider? GetScreenProvider(string id)
    {
        var path = templateLoader.GetPath(id);

        if (!File.Exists(path)) return null;

        return factory(serviceProvider, [path]);
    }
}

internal class ScribanScreenProvider(
    TemplateLoader templateLoader,
    TemplateRenderer renderer,
    XmlLayoutDeserializer layoutDeserializer,
    string path)
    : IScreenProvider
{
    public async Task<SKBitmap> GetImageAsync()
    {
        var fileContent = await templateLoader.LoadAsync(path);
        using var xml = await renderer.RenderToStreamAsync(fileContent);
        using var result = layoutDeserializer.DeserializeXml(xml);

        return DrawManager.Draw(
            result.Size,
            result.Element
        );
    }
}