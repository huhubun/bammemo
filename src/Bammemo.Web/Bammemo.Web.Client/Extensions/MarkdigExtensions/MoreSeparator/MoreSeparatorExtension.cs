using Markdig;
using Markdig.Parsers;
using Markdig.Renderers;
using Bammemo.Web.Client.Extensions.MarkdigExtensions.MoreSeparator;

#pragma warning disable IDE0130 // 命名空间与文件夹结构不匹配
namespace Bammemo.MarkdigExtensions;
#pragma warning restore IDE0130 // 命名空间与文件夹结构不匹配
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
