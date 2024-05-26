using Jint;
using Jint.Native.Function;

namespace DisplayUtil.EcmaScript.Environment;

public interface IJsExporter
{
    /// <summary>
    /// Exposes an object to the JavaScript Runtime
    /// </summary>
    /// <param name="variableName">Name of the global variable</param>
    /// <param name="obj">Affected object</param>
    void ExposeValue(string variableName, object obj);

    /// <summary>
    /// Exposes a Function to the JavaScript Runtime 
    /// </summary>
    /// <param name="functionName">Name of the function</param>
    /// <param name="function">Function</param>
    void ExposeFunction(string functionName, Function function);
    /// <summary>
    /// Exposes a Function to the JavaScript Runtime
    /// </summary>
    /// <param name="functionName">Name of the function</param>
    /// <param name="factory">Factory Delegate</param>
    void ExposeFunction(string functionName, Func<Engine, Function> factory);

    /// <summary>
    /// Exposes a Creator Function (a function which can set all public fields with an JsObject)
    /// for the given object
    /// </summary>
    /// <typeparam name="TType">Type</typeparam>
    /// <param name="functionName">Name of the function</param>
    void ExposeCreatorFunction<TType>(string functionName);

    /// <summary>
    /// Adds alls types of the namespace as a function to the engine
    /// </summary>
    /// <typeparam name="TRefType">Reference Type of namespace</typeparam>
    void ExposeNamespaceFunctionsAsCreators<TRefType>();
}
