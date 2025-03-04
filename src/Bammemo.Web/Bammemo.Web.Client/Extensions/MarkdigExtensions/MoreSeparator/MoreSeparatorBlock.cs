using Markdig.Parsers;
using Markdig.Syntax;

namespace Bammemo.Web.Client.Extensions.MarkdigExtensions.MoreSeparator;

public class MoreSeparatorBlock : LeafBlock
{
    public MoreSeparatorBlock(BlockParser parser) : base(parser) 
    {
        IsOpen = false; // 立即关闭块
    }
}
