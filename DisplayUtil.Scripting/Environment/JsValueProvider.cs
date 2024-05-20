namespace DisplayUtil.EcmaScript.Environment;

/// <summary>
/// Interface for providing values for the JavaScript runtime
/// </summary>
public interface IJsValueProvider
{
    /// <summary>
    /// Injects the Properties into JavaScript
    /// </summary>
    /// <param name="exporter">Exporter</param>
    void Inject(JsExporter exporter);
}