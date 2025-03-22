using Bammemo.Service.Options;
using Microsoft.Extensions.Options;
using Moq;

namespace Bammemo.Service.Test;

public class SecurityServiceTest
{
    private const string TEST_AES_KEY = "cHC7p25pO7sqPYOIZUMBjfeyiDOLcopTqpOgN2tFLr4=";
    private const string TEST_PLAIN_TEXT = "Bammemo";
    private const string TEST_CIPHER_TEXT = "bWtiVrOrxB46WEjP1jSDDc8/VhDT1OCJBbx3Fqvr6po=";

    private readonly Mock<IOptions<BammemoOptions>> _defaultBammemoOptions;

    public SecurityServiceTest()
    {
        _defaultBammemoOptions = new Mock<IOptions<BammemoOptions>>();
        _defaultBammemoOptions.Setup(options => options.Value).Returns(new BammemoOptions
        {
            ConnectionString = String.Empty,
            ApiUrl = String.Empty,
            Username = String.Empty,
            Password = String.Empty,
            StoragePath = String.Empty,
            Key = TEST_AES_KEY
        });
    }

    [Fact]
    public void EncryptDecryptTest()
    {
        var securityService = new SecurityService(_defaultBammemoOptions.Object);

        var cipherText = securityService.Encrypt(TEST_PLAIN_TEXT);
        Assert.NotNull(cipherText);
        Assert.NotEmpty(cipherText);

        var decryptText = securityService.Decrypt(cipherText);
        Assert.Equal(TEST_PLAIN_TEXT, decryptText);
    }

    [Fact]
    public void DecodeTest()
    {
        // Arrange
        var securityService = new SecurityService(_defaultBammemoOptions.Object);

        // Act
        var result = securityService.Decrypt(TEST_CIPHER_TEXT);

        // Assert
        Assert.Equal(TEST_PLAIN_TEXT, result);
    }
}
