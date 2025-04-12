using Markdig.Syntax.Inlines;

namespace Bammemo.Service.Abstractions.Extensions.MarkdigExtensions;

public class HashtagInline(string tag) : LeafInline
{
    public string Tag { get; } = tag;
}
