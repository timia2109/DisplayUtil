using DisplayUtil.Layouting;
using SkiaSharp;

namespace DisplayUtil.XmlModel;

public record SerializingResult(
    Element Element,
    SKSize Size
) : IDisposable
{
    public void Dispose()
    {
        Element.Dispose();
    }
}