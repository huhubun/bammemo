using Bammemo.Service.Helpers;

namespace Bammemo.Service.Test.Helpers;

public class IdHelperTest
{
    private const string CODES = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    [Fact]
    public void GenerateIdAlphabetTest()
    {
        var idAlphabet = IdHelper.GenerateIdAlphabet();

        Assert.NotNull(idAlphabet);
        Assert.NotEmpty(idAlphabet);
        Assert.Equal(CODES.Length, idAlphabet.Length);
        Assert.Equal(CODES.Order().ToArray(), idAlphabet.Order().ToArray());
    }

    [Fact]
    public void GenerateIdAlphabetTest_Different()
    {
        var idAlphabet1 = IdHelper.GenerateIdAlphabet();
        var idAlphabet2 = IdHelper.GenerateIdAlphabet();

        Assert.NotEqual(idAlphabet1, idAlphabet2);
    }
}
