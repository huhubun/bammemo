using Bammemo.Web.Client.Extensions.MarkdigExtensions.MoreSeparator.Blocks;
using Markdig.Parsers;
using Markdig.Syntax;

namespace Bammemo.Web.Client.Extensions.MarkdigExtensions.MoreSeparator.Parsers;

public class MoreSeparatorParser : BlockParser
{
    public MoreSeparatorParser()
    {
        OpeningCharacters = ['[']; // 触发解析的起始字符
    }

    public override BlockState TryOpen(BlockProcessor processor)
    {
        // 检测整行是否为 [[more]]
        var line = processor.Line.ToString().Trim();
        if (line != "[[more]]")
        {
            return BlockState.None;
        }

        // 创建分隔符块
        var block = new MoreSeparatorBlock(this)
        {
            Span = new SourceSpan(processor.Line.Start, processor.Line.End),
            IsOpen = false // 关键：立即关闭块
        };
        processor.NewBlocks.Push(block);

        return BlockState.BreakDiscard;
    }
}
