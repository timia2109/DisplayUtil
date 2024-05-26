using System.Reflection;
using Jint;
using Jint.Native;

namespace DisplayUtil.EcmaScript.PropertyObjects;

/// <summary>
/// Represents a converted property for an array as target
/// </summary>
internal class ArrayJsProp : JsProp
{
    /// <summary>
    /// Type of the array contents
    /// </summary>
    private readonly Type _itemType;

    public ArrayJsProp(Engine engine, FieldInfo targetProp) : base(engine, targetProp)
    {
        if (!Matches(_targetType))
        {
            throw new Exception("Requires collection");
        }

        _itemType = _targetType.GetElementType()!;
    }

    protected override object? ConvertValue(JsValue jsValue)
    {
        var arr = jsValue.AsArray();
        var items = Array.CreateInstance(_itemType, arr.Length);

        for (var i = 0; i < arr.Length; i++)
        {
            items.SetValue(ConvertValue(arr[i].ToObject()!, _itemType), i);
        }

        return ConvertValue(items, _targetType);
    }

    /// <summary>
    /// Is this property an array?
    /// </summary>
    /// <param name="t">Type of property</param>
    /// <returns>Is array</returns>
    internal static bool Matches(Type t)
    {
        return t.IsArray;
    }
}
