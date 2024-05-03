using Scriban;
using Scriban.Runtime;

namespace DisplayUtil.Template;

/// <summary>
/// Enum for the Scope of the Template
/// </summary>
public enum EnrichScope
{
    /// <summary>
    /// This template will used for ScreenRendering
    /// </summary>
    ScreenRendering,

    /// <summary>
    /// This template will used for Providing an other template
    /// </summary>
    TemplateProvider,

    /// <summary>
    /// This template will used as Job
    /// </summary>
    Job
}

/// <summary>
/// Extends the <see cref="TemplateContext"/>
/// </summary>
public interface ITemplateExtender
{
    /// <summary>
    /// Enriches the Template 
    /// </summary>
    /// <param name="scriptObject">The used Context</param>
    /// <param name="scope">Scope of the enrichment</param>
    void Enrich(ScriptObject scriptObject, EnrichScope scope);
}