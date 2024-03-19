using System.Xml.Serialization;
using DisplayUtil.Layouting;
using DisplayUtil.Serializing.Models;
using DisplayUtil.Utils;

namespace DisplayUtil.Serializing;

public class XmlLayoutDeserializer
{

    private readonly XmlSerializer _serializer;

    public XmlLayoutDeserializer()
    {
        var subtypes = GetType().Assembly
            .DefinedTypes
            .Where(e => e.IsAssignableTo(typeof(IXmlModel)) && e != typeof(IXmlModel))
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
    }

    public Element DeserializeXml(Stream xmlStream, FaIconDrawer iconDrawer,
        FontProvider fontProvider)
    {
        if (_serializer.Deserialize(xmlStream) is not Screen model)
        {
            throw new Exception("Unable to parse!");
        }

        return model.AsElement(iconDrawer, fontProvider);
    }

}