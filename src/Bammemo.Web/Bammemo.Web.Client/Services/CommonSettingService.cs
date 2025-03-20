using AutoMapper;
using Bammemo.Service.Abstractions.Dtos.Settings;

namespace Bammemo.Web.Client.Services;

public class CommonSettingService(
    IMapper mapper,
    Bammemo.Web.Client.WebApis.Client.WebApiClient client
    ) : ICommonSettingService
{
    public async Task<GetSettingByKeyDto> GetByKeyAsync(string key)
    {
        var response = await client.Api.Settings[key].GetAsync();
        return mapper.Map<GetSettingByKeyDto>(response);
    }

    public async Task<BatchGetSettingByKeyDto> GetByKeysAsync(IEnumerable<string> keys)
    {
        var response = await client.Api.Settings.Batch.GetAsync(c => c.QueryParameters.Keys = [.. keys]);
        return mapper.Map<BatchGetSettingByKeyDto>(response);
    }
}
