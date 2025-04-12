using Bammemo.Service.Abstractions.Extensions.MarkdigExtensions;
using Markdig;
using Markdig.Renderers;

#pragma warning disable IDE0130 // 命名空间与文件夹结构不匹配
namespace Bammemo.MarkdigExtensions;
#pragma warning restore IDE0130 // 命名空间与文件夹结构不匹配

public class HashTagExtension : IMarkdownExtension
{
    public void Setup(MarkdownPipelineBuilder pipeline)
    {
        if (!pipeline.InlineParsers.Contains<HashtagInlineParser>())
        {
            pipeline.InlineParsers.Insert(0, new HashtagInlineParser());
        }
    }

    public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
    {
        if (!renderer.ObjectRenderers.Contains<HashtagInlineRenderer>())
        {
            renderer.ObjectRenderers.Insert(0, new HashtagInlineRenderer());
        }
    }
}
