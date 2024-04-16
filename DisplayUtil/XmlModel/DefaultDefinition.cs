using System.Xml.Serialization;

namespace DisplayUtil.XmlModel;

/// <summary>
/// Setting defaults for the child elements
/// </summary>
public record struct DefaultDefinition()
{
    /// <summary>
    /// Textsize
    /// </summary>
    [XmlAttribute]
    public int TextSize;

    /// <summary>
    /// Font
    /// </summary>
    [XmlAttribute]
    public string? Font;

    /// <summary>
    /// Icon Height
    /// </summary>
    [XmlAttribute]
    public int IconHeight;

    /// <summary>
    /// Overrides the defaults
    /// </summary>
    /// <param name="other">Defined defaults on that Element</param>
    /// <returns>The merged</returns>
    public DefaultDefinition MergeWith(DefaultDefinition? other)
    {
        if (other == null) return this;
        var o = other.Value;

        return new DefaultDefinition
        {
            TextSize = o.TextSize != 0 ? o.TextSize : TextSize,
            Font = o.Font ?? Font,
            IconHeight = o.IconHeight != 0 ? o.IconHeight : IconHeight
        };
    }

    /// <summary>
    /// The default set
    /// </summary>
    public static DefaultDefinition Default =>
         new()
         {
             TextSize = 20,
             Font = "Roboto-Medium",
             IconHeight = 20
         };


}