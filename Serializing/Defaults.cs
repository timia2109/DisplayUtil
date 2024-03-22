using System.Xml.Serialization;

namespace DisplayUtil.Serializing;

/// <summary>
/// Setting defaults for the child elements
/// </summary>
public record struct DefaultDefinition()
{
    /// <summary>
    /// Textsize
    /// </summary>
    [XmlAttribute]
    public int? TextSize;

    /// <summary>
    /// Font
    /// </summary>
    [XmlAttribute]
    public string? Font;

    /// <summary>
    /// Icon Height
    /// </summary>
    [XmlAttribute]
    public int? IconHeight;

    /// <summary>
    /// Overrides the defaults
    /// </summary>
    /// <param name="other">Defined defaults on that Element</param>
    /// <returns>The merged</returns>
    public DefaultDefinition MergeWith(DefaultDefinition? other)
    {
        if (other == null) return this;

        return new DefaultDefinition
        {
            TextSize = other.Value.TextSize ?? TextSize,
            Font = other.Value.Font ?? Font,
            IconHeight = other.Value.IconHeight ?? IconHeight
        };
    }

    /// <summary>
    /// The default set
    /// </summary>
    public static DefaultDefinition Default =>
         new()
         {
             TextSize = 20,
             Font = "Roboto-Mono",
             IconHeight = 20
         };


}