// Fluent UI Blazor MarkdownSectionPreCodeExtension.cs

using Markdig.Renderers;
using Markdig;
using Markdig.Renderers.Html;
using Bammemo.Web.Client.Extensions.MarkdigExtensions.MarkdownSectionPreCode;

#pragma warning disable IDE0130 // 命名空间与文件夹结构不匹配
namespace Bammemo.Web.Client.Extensions.MarkdigExtensions;
#pragma warning restore IDE0130 // 命名空间与文件夹结构不匹配

internal class MarkdownSectionPreCodeExtension : IMarkdownExtension
{
    public void Setup(MarkdownPipelineBuilder pipeline)
    {
    }

    public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
    {
        var htmlRenderer = renderer as TextRendererBase<HtmlRenderer>;
        if (htmlRenderer == null)
        {
            return;
        }

        var originalCodeBlockRenderer = htmlRenderer.ObjectRenderers.FindExact<CodeBlockRenderer>();
        if (originalCodeBlockRenderer != null)
        {
            htmlRenderer.ObjectRenderers.Remove(originalCodeBlockRenderer);
        }

        htmlRenderer.ObjectRenderers.AddIfNotAlready(new MarkdownSectionPreCodeRenderer(
                new MarkdownSectionPreCodeRendererOptions
                {
                    PreTagAttributes = "{.snippet .hljs-copy-wrapper}",
                })
            );
    }
}
