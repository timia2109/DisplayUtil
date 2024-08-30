using DisplayUtil.EcmaScript.Environment;
using Jint;
using Jint.Native;
using Jint.Native.Function;
using Jint.Runtime;

namespace DisplayUtil.HomeAssistant.Registration;

public class HaJsValueProvider(HaStateRegistry registry) : IJsValueProvider
{
    private string[]? _lastDisposedStates;

    public void AfterSetup()
    {
        var currentStates = registry.Registrations.Keys.ToArray();
        if (_lastDisposedStates != null)
        {
            var removedStates = _lastDisposedStates.Except(currentStates).ToArray();
            foreach (var removedState in removedStates)
            {
                registry.UnregisterState(removedState);
            }
            _lastDisposedStates = null;
        }
    }

    public void OnDispose()
    {
        _lastDisposedStates = registry.Registrations.Keys.ToArray();
    }

    public void OnSetup(IJsExporter exporter)
    {
        const string functionName = "registerHassState";
        exporter.ExposeFunction(functionName,
            new RegisterHassStateFunction(
                exporter.Engine, null, new JsString(functionName), registry));
    }
}

/// <summary>
/// Function to register a HomeAssistant state.
/// <js-api>
///     registerHassState(entityId: string): HassRegistration
///     registerHassState(...entityIds: string[]): HassRegistration[]
/// </js-api>
/// </summary>
internal class RegisterHassStateFunction(Engine engine,
    Realm realm,
    JsString? name,
    HaStateRegistry registry
) : Function(engine, realm, name)
{
    protected override JsValue Call(JsValue thisObject, JsValue[] arguments)
    {
        var registrations = new List<HassRegistration>();
        foreach (var argument in arguments)
        {
            if (argument is not JsString jsString)
            {
                throw new ArgumentException("Expected string argument");
            }

            var entityId = jsString.ToString();
            var registration = registry.RegisterState(entityId);
            registrations.Add(registration);
        }

        var jsRegistrations = registrations
            .Select(r => JsValue.FromObject(Engine, r))
            .ToArray();

        if (jsRegistrations.Length == 1)
        {
            return jsRegistrations[0];
        }

        return JsArray.FromObject(Engine, jsRegistrations);
    }
}