using SkiaSharp;

namespace DisplayUtil.Layouting;

public abstract class ElementCollection : Element
{
    /// <summary>
    /// Children of this collection
    /// </summary>
    public List<Element> Children { get; } = [];

    /// <summary>
    /// Adds a new Element to this Collection
    /// </summary>
    /// <param name="element">The element</param>
    public ElementCollection Append(Element element)
    {
        Children.Add(element);
        return this;
    }

    /// <summary>
    /// Calculates the sizes of the children
    /// </summary>
    /// <param name="drawContext">Draw Context</param>
    /// <returns>Children Size Information</returns>
    protected ChildrenSizes GetChildrenSizes(DrawContext drawContext)
    {
        return Children.Aggregate(new ChildrenSizes(),
            (sum, e) => sum + e.GetSize(drawContext)
        );
    }

    protected record struct ChildrenSizes
    {
        public float WidthSum { get; init; }
        public float HeightSum { get; init; }
        public float MaxHeight { get; init; }
        public float MaxWidth { get; init; }

        public static ChildrenSizes operator +(ChildrenSizes sizes, SKSize elementSize)
        {
            return new ChildrenSizes
            {
                HeightSum = sizes.HeightSum + elementSize.Height,
                WidthSum = sizes.WidthSum + elementSize.Width,
                MaxHeight = Math.Max(sizes.MaxHeight, elementSize.Height),
                MaxWidth = Math.Max(sizes.MaxWidth, elementSize.Width)
            };
        }
    }
}
