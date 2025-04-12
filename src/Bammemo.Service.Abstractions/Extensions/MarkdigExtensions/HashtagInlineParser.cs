using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;
using System.Text;

namespace Bammemo.Service.Abstractions.Extensions.MarkdigExtensions;

public class HashtagInlineParser : InlineParser
{
    public HashtagInlineParser()
    {
        OpeningCharacters = ['#'];
    }

    public override bool Match(InlineProcessor processor, ref StringSlice slice)
    {
        // 跳过代码块
        if (processor.Block is FencedCodeBlock || processor.Block is CodeBlock)
        {
            return false;
        }

        int startPosition = slice.Start;

        // 确保 '#' 后面有字符且不是空格
        char nextChar = slice.PeekCharExtra(1);
        if (nextChar == '\0' || char.IsWhiteSpace(nextChar))
        {
            return false;
        }

        // 跳过初始的 '#'
        slice.NextChar();

        var tagBuilder = new StringBuilder();
        bool foundClosingHash = false;

        while (!slice.IsEmpty)
        {
            char currentChar = slice.CurrentChar;

            if (currentChar == '\n' || currentChar == '\r')
            {
                // 遇到换行符，视为标签未闭合，恢复起始位置
                slice.Start = startPosition;
                return false;
            }
            else if (currentChar == '\\')
            {
                // 处理转义字符
                char escapedChar = slice.PeekCharExtra(1);
                if (escapedChar != '\0')
                {
                    tagBuilder.Append(escapedChar);
                    slice.NextChar(); // 跳过转义符
                    slice.NextChar(); // 跳过被转义的字符
                }
                else
                {
                    break;
                }
            }
            else if (currentChar == '#')
            {
                // 找到结束的 '#'
                foundClosingHash = true;
                slice.NextChar(); // 跳过结束的 '#'
                break;
            }
            else
            {
                tagBuilder.Append(currentChar);
                slice.NextChar();
            }
        }

        if (!foundClosingHash || tagBuilder.Length == 0)
        {
            slice.Start = startPosition;
            return false;
        }

        string tag = tagBuilder.ToString().Trim();
        processor.Inline = new HashtagInline(tag);
        return true;
    }
}
