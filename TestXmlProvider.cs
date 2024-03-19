using DisplayUtil.Layouting;
using DisplayUtil.Scenes;
using DisplayUtil.Serializing;
using DisplayUtil.Utils;
using SkiaSharp;

namespace DisplayUtil;

internal class TestXmlProvider(XmlLayoutDeserializer xml, FaIconDrawer iconDrawer, FontProvider fontProvider) : IScreenProvider
{
    public Task<SKBitmap> GetImageAsync()
    {
        var filePath = @".\Resources\screens\main.sbntxt";
        using var stream = File.OpenRead(filePath);

        var element = xml.DeserializeXml(stream, iconDrawer, fontProvider);

        return Task.FromResult(
            DrawManager.Draw(
                Constants.EPaperDisplaySize,
                element
            )
        );
    }
}