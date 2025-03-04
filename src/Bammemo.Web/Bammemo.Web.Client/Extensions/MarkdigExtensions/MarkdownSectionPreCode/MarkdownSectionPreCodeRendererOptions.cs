// Fluent UI Blazor MarkdownSectionPreCodeRendererOptions.cs

namespace Bammemo.Web.Client.Extensions.MarkdigExtensions.MarkdownSectionPreCode;

/// <summary>
/// Options for MarkdownSectionPreCodeRenderer
/// </summary>
internal class MarkdownSectionPreCodeRendererOptions
{
    /// <summary>
    /// html attributes for Tag element in markdig generic attributes format
    /// </summary>
    public string? PreTagAttributes;
    /// <summary>
    /// html attributes for Code element in markdig generic attributes format
    /// </summary>
    public string? CodeTagAttributes;
}
