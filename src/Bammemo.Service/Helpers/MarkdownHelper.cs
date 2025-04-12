using Bammemo.MarkdigExtensions;
using Bammemo.Service.Abstractions.Extensions.MarkdigExtensions;
using Markdig;
using Markdig.Syntax;

namespace Bammemo.Service.Helpers;

public static partial class MarkdownHelper
{
    public static string[] ExtractTags(string markdown)
    {
        var pipeline = new MarkdownPipelineBuilder()
            .Use<HashTagExtension>()
            .Build();

        var document = Markdown.Parse(markdown, pipeline);

        return [.. document.Descendants<HashtagInline>().Select(inline => inline.Tag).Distinct()];
    }
}
