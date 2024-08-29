using System.Reflection;
using Jint;
using Jint.Native;
using Jint.Runtime;

namespace DisplayUtil.Scripting.PropertyObjects.Mapping;

public static class MappingFactory
{

    public static PropertyMapping ForMemberInfo(MemberInfo member)
    {
        return member switch
        {
            FieldInfo field => ForFieldInfo(field),
            PropertyInfo property => ForPropertyInfo(property),
            _ => throw new ArgumentException("MemberInfo must be a FieldInfo or PropertyInfo")
        };
    }

    private static PropertyMapping ForPropertyInfo(PropertyInfo property)
    {
        return new PropertyMapping()
        {
            PropertyName = property.Name,
            Mapping = ForType(property.PropertyType),
            Member = property
        };
    }

    private static PropertyMapping ForFieldInfo(FieldInfo field)
    {
        return new PropertyMapping()
        {
            PropertyName = field.Name,
            Mapping = ForType(field.FieldType),
            Member = field
        };
    }

    public static IMapping ForType(Type type)
    {
        switch (Type.GetTypeCode(type))
        {
            case TypeCode.Boolean:
                return new BooleanMapping();
            case TypeCode.String:
                return new StringMapping();
            case TypeCode.UInt64:
            case TypeCode.Int64:
            case TypeCode.UInt32:
            case TypeCode.Int32:
            case TypeCode.UInt16:
            case TypeCode.Int16:
            case TypeCode.Byte:
            case TypeCode.SByte:
            case TypeCode.Decimal:
            case TypeCode.Double:
            case TypeCode.Single:
                return new NumberMapping(type);
            case TypeCode.Object:
                if (type.IsEnum)
                    return new EnumMapping(type);
                if (type.IsArray)
                    return new ArrayMapping(type.GetElementType()!, ForType(type.GetElementType()!));
                return new ObjectMapping(type, GetPropertyMappings(type));
        }
    }
}

public sealed class TypeException(string expected, Types actual)
    : Exception($"Expected {expected} but got {actual}")
{
    public TypeException(Types expected, Types actual)
        : this(expected.ToString(), actual) { }
}

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

internal class NumberMapping(Type expectedType) : BaseMapping
{
    protected override Types ExpectedType => Types.Number;

    protected override object MapInternal(JsValue jsValue)
    {
        var d = jsValue.AsNumber();
        return Convert.ChangeType(d, expectedType);
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

internal class EnumMapping(Type enumType) : BaseMapping
{
    protected override Types ExpectedType => Types.String;

    protected override object MapInternal(JsValue jsValue)
    {
        return Enum.Parse(enumType, jsValue.AsString());
    }
}

internal class ArrayMapping(Type itemType, IMapping itemMapping) : IMapping
{
    public object Map(JsValue jsValue)
    {
        if (!jsValue.IsArray())
            throw new TypeException("Array", jsValue.Type);

        var arr = jsValue.AsArray();
        var items = Array.CreateInstance(itemType, arr.Length);

        for (var i = 0; i < arr.Length; i++)
            items.SetValue(itemMapping.Map(arr[i]), i);

        return items;
    }
}

/// <summary>
/// Wraps a mapping for a property
/// </summary>
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

/// <summary>
/// Wraps a Mapping for a complete Object
/// </summary>
/// <typeparam name="TType">Type of target</typeparam>
/// <param name="children">Properties</param>
internal class ObjectMapping<TType>(PropertyMapping[] children) : BaseMapping
    where TType : new()
{
    protected override Types ExpectedType => Types.Object;

    protected override object MapInternal(JsValue jsValue)
    {
        var plainObject = jsValue.ToObject();

        if (plainObject is not null &&
            typeof(TType).IsAssignableFrom(plainObject.GetType()))
            return plainObject;

        var jsObject = (JsObject)jsValue;
        var obj = new TType();

        foreach (var child in children)
            child.MapProperty(obj, jsObject);

        return obj;
    }
}
