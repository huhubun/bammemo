using Bammemo.Web.Client.Extensions.MarkdigExtensions.MoreSeparator.Blocks;
using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace Bammemo.Web.Client.Extensions.MarkdigExtensions.MoreSeparator.Renderers;

public class HtmlMoreSeparatorRenderer : HtmlObjectRenderer<MoreSeparatorBlock>
{
    protected override void Write(HtmlRenderer renderer, MoreSeparatorBlock obj)
    {
        // 在完整页面渲染时不输出任何内容
        // 在摘要渲染时通过检测块存在性来截断
    }
}
