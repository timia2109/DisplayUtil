using System.Reflection;
using Jint;
using Jint.Native;

namespace DisplayUtil.EcmaScript.PropertyObjects;

/// <summary>
/// Converter for Enums
/// </summary>
internal class EnumJsProp : JsProp
{
    public EnumJsProp(Engine engine, FieldInfo TargetProperty) : base(engine, TargetProperty)
    {
    }

    protected override object? ConvertValue(JsValue jsValue)
    {
        return Enum.Parse(_targetType, jsValue.AsString());
    }

    /// <summary>
    /// Is this property an array?
    /// </summary>
    /// <param name="t">Type of property</param>
    /// <returns>Is array</returns>
    internal static bool Matches(Type t)
    {
        return t.IsEnum;
    }
}