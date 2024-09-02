using System.Reflection;
using System.Text.Json;
using Jint;
using Jint.Native;
using Jint.Runtime;

namespace DisplayUtil.Scripting.PropertyObjects.Mapping;

public sealed class MappingFactory(MappingRegistry registry)
{
    private static readonly JsonNamingPolicy _namingPolicy = JsonNamingPolicy.CamelCase;

    internal PropertyMapping ForMemberInfo(MemberInfo member)
    {
        return member switch
        {
            FieldInfo field => ForFieldInfo(field),
            PropertyInfo property => ForPropertyInfo(property),
            _ => throw new ArgumentException("MemberInfo must be a FieldInfo or PropertyInfo")
        };
    }

    private PropertyMapping ForPropertyInfo(PropertyInfo property)
    {
        return new PropertyMapping()
        {
            PropertyName = _namingPolicy.ConvertName(property.Name),
            Mapping = ForType(property.PropertyType),
            Member = property
        };
    }

    private PropertyMapping ForFieldInfo(FieldInfo field)
    {
        return new PropertyMapping()
        {
            PropertyName = _namingPolicy.ConvertName(field.Name),
            Mapping = ForType(field.FieldType),
            Member = field
        };
    }

    private PropertyMapping[] GetPropertyMappings(Type type)
    {
        return type.GetMembers(BindingFlags.Instance | BindingFlags.Public)
            .Where(m => m is PropertyInfo || m is FieldInfo)
            .Select(ForMemberInfo)
            .ToArray();
    }

    public IMapping ForObject(Type type)
    {
        if (type.GetConstructor(Type.EmptyTypes) == null)
        {
            return OptimisticObjectMapping.Instance;
        }

        var genericType = typeof(ObjectMapping<>).MakeGenericType(type);
        var constructor = genericType.GetConstructor([typeof(PropertyMapping[])]);
        var mappings = GetPropertyMappings(type);
        return (IMapping)constructor!.Invoke([mappings]);
    }

    public IMapping ForType(Type type)
    {
        if (type.IsEnum)
            return new EnumMapping(type);

        switch (Type.GetTypeCode(type))
        {
            case TypeCode.Boolean:
                return BooleanMapping.Instance;
            case TypeCode.String:
                return StringMapping.Instance;
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
                if (type.IsArray)
                    return new ArrayMapping(type.GetElementType()!, ForType(type.GetElementType()!));
                return registry.RegisterType(type);
            default:
                throw new ArgumentException("Type not supported");
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
        if (jsValue.Type is Types.Undefined or Types.Null) return null;

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
    internal static StringMapping Instance = new();

    private StringMapping() { }

    protected override Types ExpectedType => Types.String;

    protected override object MapInternal(JsValue jsValue)
    {
        return jsValue.AsString();
    }
}

internal class BooleanMapping : BaseMapping
{
    internal static BooleanMapping Instance = new();

    private BooleanMapping() { }

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
    internal PropertyMapping[] Children => children;

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

internal class OptimisticObjectMapping : BaseMapping
{
    internal static OptimisticObjectMapping Instance = new();

    private OptimisticObjectMapping() { }

    protected override Types ExpectedType => Types.Object;

    protected override object MapInternal(JsValue jsValue)
    {
        return jsValue.ToObject();
    }
}