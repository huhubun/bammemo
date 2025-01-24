using Bammemo.Data.Entities;
using Bammemo.Service.Abstractions;
using Bammemo.Service.Server.Interfaces;
using Moq;

namespace Bammemo.Service.Server.Test;

public class IdServiceTest
{
    private const string ID_ALPHABET = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private readonly Mock<IServiceProvider> _defaultServiceProvider;

    public IdServiceTest()
    {
        _defaultServiceProvider = new Mock<IServiceProvider>();
        _defaultServiceProvider.Setup(s => s.GetService(typeof(ISettingService))).Returns(() =>
        {
            var settingService = new Mock<ISettingService>();
            settingService.Setup(s => s.GetByKeyAsync(SettingKeys.IdAlphabet)).ReturnsAsync(new Setting
            {
                Key = SettingKeys.IdAlphabet,
                Value = ID_ALPHABET
            });

            return settingService.Object;
        });
    }

    [Fact]
    public async Task EncodeAsyncTest()
    {
        // Arrange
        var idService = new IdService(_defaultServiceProvider.Object);

        // Act
        var result = await idService.EncodeAsync(10001);

        // Assert
        Assert.Equal("L5iGpH", result);
    }

    [Fact]
    public async Task DecodeAsyncTest()
    {
        // Arrange
        var idService = new IdService(_defaultServiceProvider.Object);

        // Act
        var result = await idService.DecodeAsync("L5iGpH");

        // Assert
        Assert.Equal(10001, result);
    }
}
