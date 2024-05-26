using System.Reflection;
using Jint;
using Jint.Native;

namespace DisplayUtil.EcmaScript.PropertyObjects;

/// <summary>
/// Represents a mapped property
/// </summary>
/// <param name="engine">JS Engine</param>
/// <param name="TargetProperty">Target Property</param>
internal class JsProp(
    Engine engine,
    FieldInfo TargetProperty
)
{
    protected Engine CurrentEngine => engine;
    protected readonly Type _targetType = TargetProperty.FieldType;

    /// <summary>
    /// Converts a object to another type
    /// </summary>
    /// <param name="obj">Affected value</param>
    /// <param name="targetType">Target Type</param>
    /// <returns>The converted C# Type</returns>
    protected object? ConvertValue(object obj, Type targetType)
    {
        if (targetType.IsAssignableFrom(obj!.GetType()))
            return obj;

        return Convert.ChangeType(obj, _targetType);
    }

    /// <summary>
    /// Converts the JavaScript value to the <see cref="_targetType"/>
    /// </summary>
    /// <param name="jsValue">JavaScript Value</param>
    /// <returns>Converted value</returns>
    protected virtual object? ConvertValue(JsValue jsValue)
    {
        return ConvertValue(jsValue.ToObject()!, _targetType);
    }

    /// <summary>
    /// Sets the value to the object
    /// </summary>
    /// <param name="jsValue">JavaScript Value</param>
    /// <param name="targetObject">Target Object</param>
    internal void SetValue(
        JsValue jsValue,
        object targetObject
    )
    {
        if (jsValue.IsUndefined() || jsValue.IsNull()) return;

        TargetProperty.SetValue(
            targetObject,
            ConvertValue(jsValue)
        );
    }
};
