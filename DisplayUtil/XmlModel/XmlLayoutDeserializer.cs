using System.Xml.Serialization;
using DisplayUtil.Layouting;
using DisplayUtil.Providers;
using DisplayUtil.Utils;
using DisplayUtil.XmlModel.Models;
using SkiaSharp;

namespace DisplayUtil.XmlModel;

public class XmlLayoutDeserializer
{
    private readonly IconDrawer _iconDrawer;
    private readonly FontProvider _fontProvider;
    private readonly XmlSerializer _serializer;

    public XmlLayoutDeserializer(IconDrawer iconDrawer, FontProvider fontProvider)
    {
        var subtypes = GetType().Assembly
            .DefinedTypes
            .Where(e => e.IsAssignableTo(typeof(IXmlModel)) && !e.IsAbstract)
            .ToArray();

        var attrOverrides = new XmlAttributeOverrides();
        var attrs = new XmlAttributes();

        foreach (var type in subtypes)
        {
            attrs.XmlElements.Add(new XmlElementAttribute
            {
                ElementName = type.Name,
                Type = type
            });
        }

        attrOverrides.Add(typeof(IXmlModel), nameof(IXmlModel.Children), attrs);

        _serializer = new XmlSerializer(typeof(Screen), attrOverrides);
        _iconDrawer = iconDrawer;
        _fontProvider = fontProvider;
    }

    public SerializingResult DeserializeXml(Stream xmlStream)
    {
        if (_serializer.Deserialize(xmlStream) is not Screen model)
        {
            throw new Exception("Unable to parse!");
        }

        return new(
            model.AsElement(_iconDrawer, _fontProvider, DefaultDefinition.Default),
            new SKSize(model.Width, model.Height)
        );
    }

}