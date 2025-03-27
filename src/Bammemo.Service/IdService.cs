using Bammemo.Service.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Sqids;

namespace Bammemo.Service;

public class IdService(
    ISettingService settingService,
    IMemoryCache memoryCache) : IIdService
{
    public async Task<string> EncodeAsync(int number) => (await SqidsTask)?.Encode(number) ?? throw new NullReferenceException(nameof(SqidsTask));
    public async Task<int> DecodeAsync(string str)
    {
        ArgumentNullException.ThrowIfNull(str);

        return (await SqidsTask)?.Decode(str).Single() ?? throw new NullReferenceException(nameof(SqidsTask));
    }

    private Task<SqidsEncoder<int>?> SqidsTask => memoryCache.GetOrCreateAsync(nameof(Sqids), async _ =>
    {
        var idAlphabetSetting = await settingService.GetByKeyFromCacheAsync(SettingKeys.IdAlphabet);

        ArgumentNullException.ThrowIfNull(idAlphabetSetting);

        return new SqidsEncoder<int>(new SqidsOptions
        {
            Alphabet = idAlphabetSetting.Value,
            MinLength = 6
        });
    });
}
