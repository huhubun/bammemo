using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace Bammemo.Service.Abstractions.Extensions.MarkdigExtensions;

public class HashtagInlineRenderer : HtmlObjectRenderer<HashtagInline>
{
    protected override void Write(HtmlRenderer renderer, HashtagInline obj)
    {
        if (renderer.EnableHtmlForInline)
        {
            renderer.Write($" <a href=\"/?tags={Uri.EscapeDataString(obj.Tag)}\">#{obj.Tag}</a> ");
        }
        else
        {
            renderer.Write($"#{obj.Tag}");
        }
    }
}
