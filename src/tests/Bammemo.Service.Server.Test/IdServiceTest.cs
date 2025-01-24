using Bammemo.Data.Entities;
using Bammemo.Service.Abstractions;
using Bammemo.Service.Server.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace Bammemo.Service.Server.Test;

public class IdServiceTest
{
    private const string ID_ALPHABET = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private readonly Mock<ISettingService> _defaultSettingService;
    private readonly MemoryCache _defaultMemoryCache = new MemoryCache(new MemoryCacheOptions());

    public IdServiceTest()
    {
        _defaultSettingService = new Mock<ISettingService>();
        _defaultSettingService.Setup(s => s.GetByKeyAsync(SettingKeys.IdAlphabet)).ReturnsAsync(new Setting
        {
            Key = SettingKeys.IdAlphabet,
            Value = ID_ALPHABET
        });
    }

    [Fact]
    public async Task EncodeAsyncTest()
    {
        // Arrange
        var idService = new IdService(_defaultSettingService.Object, _defaultMemoryCache);

        // Act
        var result = await idService.EncodeAsync(10001);

        // Assert
        Assert.Equal("L5iGpH", result);
    }

    [Fact]
    public async Task DecodeAsyncTest()
    {
        // Arrange
        var idService = new IdService(_defaultSettingService.Object, _defaultMemoryCache);

        // Act
        var result = await idService.DecodeAsync("L5iGpH");

        // Assert
        Assert.Equal(10001, result);
    }
}
