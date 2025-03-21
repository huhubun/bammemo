using Bammemo.Service.Abstractions.Dtos.Settings;

namespace Bammemo.Service.Abstractions.CommonServices;

public interface ICommonSettingService
{
    Task<GetSettingByKeyDto> GetByKeyAsync(string key);
    Task<BatchGetSettingByKeyDto> GetByKeysAsync(IEnumerable<string> keys);
}
