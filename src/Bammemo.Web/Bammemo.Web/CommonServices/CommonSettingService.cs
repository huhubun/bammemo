using Bammemo.Service.Abstractions.CommonServices;
using Bammemo.Service.Abstractions.Dtos.Settings;
using Bammemo.Service.Interfaces;

namespace Bammemo.Web.CommonServices;

public class CommonSettingService(
    ISettingService settingService
    ) : ICommonSettingService
{
    public async Task<GetSettingByKeyDto> GetByKeyAsync(string key)
    {
        var setting = await settingService.GetByKeyFromCacheAsync(key);
        return setting.MapTo<GetSettingByKeyDto>();
    }

    public async Task<BatchGetSettingByKeyDto> GetByKeysAsync(IEnumerable<string> keys)
    {
        var settings = await settingService.GetByKeysAsync(keys);
        return new BatchGetSettingByKeyDto
        {
            Settings = settings.MapToList<BatchGetSettingByKeyDto.SettingItemModel>()
        };
    }
}
