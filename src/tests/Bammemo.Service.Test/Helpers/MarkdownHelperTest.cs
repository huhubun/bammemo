using Bammemo.Service.Helpers;

namespace Bammemo.Service.Test.Helpers;

public class MarkdownHelperTest
{
    [Fact]
    public void ExtractTags_SingleTagTest()
    {
        var tags = MarkdownHelper.ExtractTags(
            """
            # h1
            this is #demo# markdown.
            """);

        Assert.Equal(["demo"], tags);
    }

    [Theory]
    [InlineData(
        """
        # h1
        this is #### not a tag.

        ## h2
        this is the #demo# tag.
        """)]
    [InlineData(
        """
        # h1
        this is #### not a tag.

        ```bash
        rm -rf #not tag
        ```

        ## h2
        this is the #demo# tag.
        """)]
    [InlineData(
        """
        # h1
        this is the #demo# tag.

        #
        # 
        #	
        #  
        #|
        #.
        #,
        #;
        #!
        #@
        #$
        #%
        #^
        #&
        #*
        #(
        #)
        """)]
    public void ExtractTags_InvalidTagTest(string markdown)
    {
        var tags = MarkdownHelper.ExtractTags(markdown);

        Assert.Equal(["demo"], tags);
    }

    [Theory]
    [InlineData(
        """
        # h1
        this is #### not a tag.

        ## h2
        this is the number #5# tag.
        """,
        "5")]
    [InlineData(
        """
        # h1
        this is #### not a tag.

        ## h2
        this is the symbol #C\## tag.
        """,
        "C#")]
    [InlineData(
        """
        # h1
        this is #### not a tag.

        ## h2
        this is the #number-5# tag.
        """,
        "number-5")]
    [InlineData(
        """
        # h1
        this is #### not a tag.

        ## h2
        this is the #asp_net_core# tag.
        """,
        "asp_net_core")]
    [InlineData(
        """
        # h1
        this is #### not a tag.

        ## h2
        this is the #ASP.NET Core# tag.
        """,
        "ASP.NET Core")]
    [InlineData(
        """
        # h1
        this is #### not a tag.

        ## h2
        this is the #中文# tag.
        """,
        "中文")]
    [InlineData(
        """
        # h1
        this is #### not a tag.

        ## h2
        this is the #あ# tag.
        """,
        "あ")]
    public void ExtractTags_TagContentTest(string markdown, string tagContent)
    {
        var tags = MarkdownHelper.ExtractTags(markdown);

        Assert.Equal([tagContent], tags);
    }

    [Theory]
    [InlineData(
    """
        # h1
        this is #not# a tag.

        ## h2
        this is the #中文# tag.
        """,
    "not", "中文")]
    [InlineData(
    """
        # h1
        this is #one# #two# #three# tags.
        """,
    "one", "two", "three")]
    [InlineData(
    """
        # h1
        this is#a##b##c#tags.
        """,
    "a", "b", "c")]
    public void ExtractTags_MultipleTagsTest(string markdown, params string[] expectedTags)
    {
        var tags = MarkdownHelper.ExtractTags(markdown);

        Assert.Equal(expectedTags, tags);
    }
}
