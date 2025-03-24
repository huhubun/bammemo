using Markdig;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System.Text.RegularExpressions;

namespace Bammemo.Service.Helpers;

public static partial class MarkdownHelper
{
    public static string[] ExtractTags(string markdown)
    {
        // 使用 Markdig 解析 Markdown
        var pipeline = new MarkdownPipelineBuilder().Build();
        var document = Markdig.Markdown.Parse(markdown, pipeline);

        var tags = new List<string>();

        // 支持汉字、日语等字符
        var tagRegex = TagNameRegex();

        // 遍历 Markdown 文档中的所有节点
        foreach (var block in document)
        {
            // 跳过标题节点（以 # 开头的节点）
            if (block is HeadingBlock headingBlock)
                continue;

            // 跳过代码块节点
            if (block is FencedCodeBlock || block is CodeBlock)
                continue;

            // 遍历 Inline 内的节点，找出可能的标签
            if (block is ParagraphBlock paragraph && paragraph.Inline != null)
            {
                foreach (var inline in paragraph.Inline)
                {
                    // 只有 LiteralInline 才是文本
                    if (inline is LiteralInline literal)
                    {
                        // 匹配合法的标签
                        var matches = tagRegex.Matches(literal.Content.ToString());
                        foreach (Match match in matches)
                        {
                            // 添加匹配到的标签到列表
                            tags.Add(match.Groups[1].Value);
                        }
                    }
                }
            }
        }

        // 移除重复的标签（去重）
        return tags.Distinct().ToArray();
    }

    [GeneratedRegex(@"#([\p{L}\p{N}_-]+)", RegexOptions.IgnoreCase, 200)]
    private static partial Regex TagNameRegex();
}
