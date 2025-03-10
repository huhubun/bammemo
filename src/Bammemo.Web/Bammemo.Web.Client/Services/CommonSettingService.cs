using AutoMapper;
using Bammemo.Service.Abstractions.Dtos.Settings;
using Bammemo.Service.Abstractions.WebApiModels.Settings;

namespace Bammemo.Web.Client.Services;

public class CommonSettingService(
    IMapper mapper,
    WebApiClient client
    ) : ICommonSettingService
{
    public async Task<GetSettingByKeyDto> GetByKeyAsync(string key)
    {
        var response = await client.Settings.GetByKeyAsync(key);
        return mapper.Map<GetSettingByKeyDto>(response);
    }

    public async Task<BatchGetSettingByKeyDto> GetByKeysAsync(IEnumerable<string> keys)
    {
        var response = await client.Settings.BatchGetByKeysAsync(new BatchGetSettingByKeyRequest
        {
            Keys = keys.ToArray()
        });

        return mapper.Map<BatchGetSettingByKeyDto>(response);
    }
}
