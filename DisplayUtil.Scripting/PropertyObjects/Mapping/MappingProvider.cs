using System.Reflection;
using Jint;
using Jint.Native;
using Jint.Runtime;

namespace DisplayUtil.Scripting.PropertyObjects.Mapping;

public class MappingProvider
{

}

public sealed class TypeException(Types expected, Types actual)
    : Exception($"Expected {expected} but got {actual}");


/// <summary>
/// Represents a mapping between a JsValue and a .NET object
/// </summary>
public interface IMapping
{
    /// <summary>
    /// Maps a JsValue to a .NET object
    /// </summary>
    /// <param name="jsValue">Value</param>
    /// <returns>.NET Value</returns>
    /// <exception cref="TypeException">Thrown when the JsValue is not of the expected type</exception>
    object Map(JsValue jsValue);
}

internal abstract class BaseMapping : IMapping
{
    public object Map(JsValue jsValue)
    {

        if (jsValue.Type != ExpectedType)
            throw new TypeException(ExpectedType, jsValue.Type);

        return MapInternal(jsValue);
    }

    protected abstract Types ExpectedType { get; }

    protected abstract object MapInternal(JsValue jsValue);
}

internal class NumberMapping : BaseMapping
{
    protected override Types ExpectedType => Types.Number;

    protected override object MapInternal(JsValue jsValue)
    {
        return jsValue.AsNumber();
    }
}

internal class StringMapping : BaseMapping
{
    protected override Types ExpectedType => Types.String;

    protected override object MapInternal(JsValue jsValue)
    {
        return jsValue.AsString();
    }
}

internal class BooleanMapping : BaseMapping
{
    protected override Types ExpectedType => Types.Boolean;

    protected override object MapInternal(JsValue jsValue)
    {
        return jsValue.AsBoolean();
    }
}

internal class PropertyMapping
{

    public required string PropertyName { get; init; }
    public required IMapping Mapping { get; init; }
    public required MemberInfo Member { get; init; }

    public void MapProperty(object target, JsObject jsObject)
    {
        if (!jsObject.HasProperty(PropertyName))
            return;

        var jsChild = jsObject.Get(PropertyName);
        var child = Mapping.Map(jsChild);

        if (Member is PropertyInfo property)
            property.SetValue(target, child);
        else if (Member is FieldInfo field)
            field.SetValue(target, child);
    }
}

internal class ObjectMapping<TType>(PropertyMapping[] children) : BaseMapping
    where TType : new()
{
    protected override Types ExpectedType => Types.Object;

    protected override object MapInternal(JsValue jsValue)
    {
        var jsObject = (JsObject)jsValue;
        var obj = new TType();

        foreach (var child in children)
            child.MapProperty(obj, jsObject);

        return obj;
    }
}
