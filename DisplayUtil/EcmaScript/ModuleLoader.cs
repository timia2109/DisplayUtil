using Jint;
using Jint.Runtime.Modules;
using Microsoft.Extensions.Options;

namespace DisplayUtil.EcmaScript;

public record JsSettings
{
    public required IReadOnlyDictionary<string, string> Paths { get; init; }
}

internal class DisplayUtilModuleLoader(IOptions<JsSettings> options) : ModuleLoader
{
    private static string[] _allowedExtensions = ["js", "mjs"];

    public override ResolvedSpecifier Resolve(
        string? referencingModuleLocation,
        ModuleRequest moduleRequest)
    {
        var settings = options.Value;

        var specifier = moduleRequest.Specifier;
        if (string.IsNullOrEmpty(specifier))
        {
            throw new ModuleResolutionException(
                "Invalid Module Specifier",
                specifier, referencingModuleLocation, null);
        }

        Uri resolved;
        if (Uri.TryCreate(specifier, UriKind.Absolute, out var uri))
        {
            var scheme = uri.Scheme;
            if (!settings.Paths.TryGetValue(scheme, out var directory))
            {
                throw new ModuleResolutionException("Unable to find scope",
                    specifier, referencingModuleLocation, null);
            }

            var path = _allowedExtensions
                .Select(e => $"{uri.AbsolutePath}.{e}")
                .Select(f => Path.Combine(directory, f))
                .First(File.Exists);

            resolved = new Uri(Path.GetFullPath(path));
        }
        else
        {
            throw new Exception("Unable to parse");
        }

        return new ResolvedSpecifier(
            moduleRequest,
            resolved.AbsoluteUri,
            resolved,
            SpecifierType.RelativeOrAbsolute
        );
    }

    protected override string LoadModuleContents(Engine engine, ResolvedSpecifier resolved)
    {
        var specifier = resolved.ModuleRequest.Specifier;

        if (resolved.Uri == null)
        {
            throw new Exception($"Module '{specifier}' of type '{resolved.Type}' has no resolved URI.");
        }

        var fileName = Uri.UnescapeDataString(resolved.Uri.AbsolutePath);
        if (!File.Exists(fileName))
        {
            throw new ModuleResolutionException("Module Not Found", specifier, parent: null, fileName);
        }

        return File.ReadAllText(fileName);
    }

    private static bool IsRelative(string specifier)
    {
        return specifier.StartsWith('.') || specifier.StartsWith('/');
    }
}