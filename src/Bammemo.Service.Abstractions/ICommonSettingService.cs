using Bammemo.Service.Abstractions.Dtos.Settings;

namespace Bammemo.Service.Abstractions;

public interface ICommonSettingService
{
    Task<GetSettingByKeyDto> GetByKeyAsync(string key);
    Task<BatchGetSettingByKeyDto> GetByKeysAsync(IEnumerable<string> keys);
}
