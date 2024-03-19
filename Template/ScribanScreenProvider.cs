using DisplayUtil.Layouting;
using DisplayUtil.Scenes;
using DisplayUtil.Serializing;
using DisplayUtil.Utils;
using SkiaSharp;

namespace DisplayUtil.Template;

public class ScribanScreenProvider(
    TemplateRenderer renderer,
    XmlLayoutDeserializer layoutDeserializer,
    string path)
    : IScreenProvider
{
    public async Task<SKBitmap> GetImageAsync()
    {
        var fileContent = await File.ReadAllTextAsync(path);
        using var xml = await renderer.RenderToStreamAsync(fileContent);
        var element = layoutDeserializer.DeserializeXml(xml);

        return DrawManager.Draw(
            Constants.EPaperDisplaySize,
            element
        );
    }
}