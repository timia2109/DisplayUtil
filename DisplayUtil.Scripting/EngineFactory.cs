using System.Globalization;
using DisplayUtil.EcmaScript;
using DisplayUtil.EcmaScript.Environment;
using DisplayUtil.Scripting.PropertyObjects.Mapping;
using Jint;
using Jint.Runtime.Modules;
using Microsoft.Extensions.Options;

namespace DisplayUtil.Scripting;

/// <summary>
/// Responsible for creating engines with the correct setup
/// </summary>
internal class EngineFactory(IModuleLoader moduleLoader,
    MappingRegistry mappingRegistry,
    IOptions<JsSettings> options,
    IEnumerable<IJsValueProvider> jsValueProviders)
{
    public Engine? CurrentEngine { get; private set; }

    /// <summary>
    /// Disposes the last engine and it ValueProviders
    /// </summary>
    private void DisposeLastEngine()
    {
        if (CurrentEngine != null)
        {
            CurrentEngine.Dispose();
            foreach (var provider in jsValueProviders)
            {
                provider.OnDispose();
            }
            CurrentEngine = null;
        }
    }

    /// <summary>
    /// Creates a new Engine
    /// </summary>
    /// <param name="cultureInfo">Based Culture</param>
    /// <returns>The Engine</returns>
    public Engine CreateEngine(CultureInfo cultureInfo)
    {
        lock (this)
        {
            DisposeLastEngine();
            var options = new Jint.Options
            {
                Culture = cultureInfo,
                Strict = true
            };
            options.Modules.ModuleLoader = moduleLoader;

            var engine = new Engine(options);
            engine.SetValue("log", new Action<object>(Console.WriteLine));
            var exporter = new JsExporter(engine, mappingRegistry);

            foreach (var provider in jsValueProviders)
            {
                provider.OnSetup(exporter);
            }

            CurrentEngine = engine;
            PreloadModules();

            return engine;
        }
    }

    private void PreloadModules()
    {
        var settings = options.Value;
        foreach (var (pathKey, path) in settings.Paths)
        {
            var files = DisplayUtilModuleLoader._allowedExtensions
                .Select(e => Directory.EnumerateFiles(path, $"*.{e}"))
                .SelectMany(f => f);

            foreach (var file in files)
            {
                var fileName = Path.GetFileNameWithoutExtension(file);

                CurrentEngine!.Modules.Import($"{pathKey}:{fileName}");
            }
        }
    }
}