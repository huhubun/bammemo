using Bammemo.Web.Client.Extensions.MarkdigExtensions.MoreSeparator.Parsers;
using Bammemo.Web.Client.Extensions.MarkdigExtensions.MoreSeparator.Renderers;
using Markdig;
using Markdig.Parsers;
using Markdig.Renderers;

namespace Bammemo.Web.Client.Extensions.MarkdigExtensions.MoreSeparator;

public class MoreSeparatorExtension : IMarkdownExtension
{
    public void Setup(MarkdownPipelineBuilder pipeline)
    {
        pipeline.BlockParsers.InsertBefore<ParagraphBlockParser>(new MoreSeparatorParser());
    }

    public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
    {
        if (renderer is HtmlRenderer htmlRenderer)
        {
            htmlRenderer.ObjectRenderers.Add(new HtmlMoreSeparatorRenderer());
        }
    }
}
