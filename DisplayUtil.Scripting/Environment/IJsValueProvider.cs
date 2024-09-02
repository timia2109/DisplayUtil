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
    void OnSetup(IJsExporter exporter);

    /// <summary>
    /// Runs after the setup of the current Engine has been done
    /// </summary>
    void AfterSetup();

    /// <summary>
    /// Runs when the current Engine gets disposed, by a module change
    /// </summary>
    void OnDispose();
}