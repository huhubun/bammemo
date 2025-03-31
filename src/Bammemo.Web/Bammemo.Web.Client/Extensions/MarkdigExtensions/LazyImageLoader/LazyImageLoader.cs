using Markdig;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

#pragma warning disable IDE0130 // 命名空间与文件夹结构不匹配
namespace Bammemo.Web.Client.Extensions.MarkdigExtensions;
#pragma warning restore IDE0130 // 命名空间与文件夹结构不匹配

public class LazyImageLoader : IMarkdownExtension
{
    public void Setup(MarkdownPipelineBuilder pipeline)
    {
        pipeline.DocumentProcessed += LazyImgPath;
    }

    public static void LazyImgPath(MarkdownDocument document)
    {
        foreach (LinkInline link in document.Descendants<LinkInline>())
        {
            if (link.IsImage)
            {
                var url = link.Url;

                // this is a APNG
                link.Url = "/loading.png";

                link.SetAttributes(new HtmlAttributes
                {
                    Classes = ["lozad bammemo-slip-content-img"],
                    Properties = [
                        KeyValuePair.Create("data-src", url)
                    ]
                });
            }
        }
    }

    public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
    {
    }
}
