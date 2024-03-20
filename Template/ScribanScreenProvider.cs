using DisplayUtil.Layouting;
using DisplayUtil.Scenes;
using DisplayUtil.Serializing;
using DisplayUtil.Utils;
using SkiaSharp;

namespace DisplayUtil.Template;

internal class ScibanScreenProviderSource(IServiceProvider serviceProvider)
    : IScreenProviderSource
{

    private const string TemplatePath = "./Resources/screens";

    private static readonly ObjectFactory<ScribanScreenProvider>
        factory = ActivatorUtilities.CreateFactory<ScribanScreenProvider>([
            typeof(string)
        ]);

    public IScreenProvider? GetScreenProvider(string id)
    {
        var path = Path.GetFullPath(
            Path.Combine(TemplatePath, $"{id}.sbntxt")
        );

        if (!File.Exists(path)) return null;

        return factory(serviceProvider, [path]);
    }
}

internal class ScribanScreenProvider(
    TemplateRenderer renderer,
    XmlLayoutDeserializer layoutDeserializer,
    string path)
    : IScreenProvider
{
    public async Task<SKBitmap> GetImageAsync()
    {
        var fileContent = await File.ReadAllTextAsync(path);
        using var xml = await renderer.RenderToStreamAsync(fileContent);
        using var element = layoutDeserializer.DeserializeXml(xml);

        return DrawManager.Draw(
            Constants.EPaperDisplaySize,
            element
        );
    }
}